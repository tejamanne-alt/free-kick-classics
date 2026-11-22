namespace FreeKickClassics.Core.Physics
{
    /// <summary>
    /// Finds the launch direction that sends a ball with a given speed and spin through a chosen point on the goal-line
    /// plane, through the full flight model (drag, swerve, knuckle wobble included). Newton's method on yaw and pitch.
    /// </summary>
    public static class AimSolver
    {
        /// <summary>How close to the target the crossing must be, in metres.</summary>
        public const double Tolerance = 0.002;

        const int MaxIterations = 30;
        const double DerivativeStep = 1e-5;
        const double MaxAngleStep = 0.15;

        public static bool TrySolve(
            FlightModel model,
            Vec3 start,
            double speed,
            Vec3 spin,
            ulong knuckleSeed,
            double goalLineZ,
            double targetX,
            double targetY,
            out Vec3 velocity)
        {
            velocity = Vec3.Zero;
            double dx = targetX - start.X;
            double dz = goalLineZ - start.Z;
            if (dz <= 0 || speed <= 0) return false;
            double horizontal = System.Math.Sqrt(dx * dx + dz * dz);

            if (!TryVacuumLowPitch(speed, horizontal, targetY - start.Y, out double pitch)) return false;
            double yaw = System.Math.Atan2(dx, dz);

            for (int iteration = 0; iteration < MaxIterations; iteration++)
            {
                if (!TryResidual(model, start, speed, spin, knuckleSeed, goalLineZ, targetX, targetY, yaw, pitch, out double rx, out double ry))
                {
                    pitch += 0.05; // Fell short of the goal line: aim higher.
                    continue;
                }

                if (rx * rx + ry * ry <= Tolerance * Tolerance)
                {
                    velocity = Direction(yaw, pitch) * speed;
                    return true;
                }

                if (!TryResidual(model, start, speed, spin, knuckleSeed, goalLineZ, targetX, targetY, yaw + DerivativeStep, pitch, out double rxYaw, out double ryYaw) ||
                    !TryResidual(model, start, speed, spin, knuckleSeed, goalLineZ, targetX, targetY, yaw, pitch + DerivativeStep, out double rxPitch, out double ryPitch))
                {
                    pitch += 0.02;
                    continue;
                }

                double j00 = (rxYaw - rx) / DerivativeStep;
                double j01 = (rxPitch - rx) / DerivativeStep;
                double j10 = (ryYaw - ry) / DerivativeStep;
                double j11 = (ryPitch - ry) / DerivativeStep;
                double determinant = j00 * j11 - j01 * j10;
                if (System.Math.Abs(determinant) < 1e-12) return false;

                double stepYaw = -(j11 * rx - j01 * ry) / determinant;
                double stepPitch = -(-j10 * rx + j00 * ry) / determinant;
                yaw += Clamp(stepYaw, -MaxAngleStep, MaxAngleStep);
                pitch = Clamp(pitch + Clamp(stepPitch, -MaxAngleStep, MaxAngleStep), -0.4, 1.3);
            }

            return false;
        }

        public static Vec3 Direction(double yaw, double pitch)
        {
            double cosPitch = System.Math.Cos(pitch);
            return new Vec3(System.Math.Sin(yaw) * cosPitch, System.Math.Sin(pitch), System.Math.Cos(yaw) * cosPitch);
        }

        static bool TryResidual(
            FlightModel model,
            Vec3 start,
            double speed,
            Vec3 spin,
            ulong knuckleSeed,
            double goalLineZ,
            double targetX,
            double targetY,
            double yaw,
            double pitch,
            out double rx,
            out double ry)
        {
            var launch = new LaunchParams(start, Direction(yaw, pitch) * speed, spin, knuckleSeed);
            var result = Flight.Simulate(model, launch, goalLineZ);
            rx = ry = 0;
            if (!result.GoalLine.Happened) return false;
            rx = result.GoalLine.Point.X - targetX;
            ry = result.GoalLine.Point.Y - targetY;
            return true;
        }

        /// <summary>The flatter of the two no-air projectile solutions, as a starting guess.</summary>
        static bool TryVacuumLowPitch(double speed, double horizontal, double rise, out double pitch)
        {
            const double g = FlightModel.Gravity;
            double v2 = speed * speed;
            double discriminant = v2 * v2 - g * (g * horizontal * horizontal + 2 * rise * v2);
            pitch = 0;
            if (discriminant < 0) return false;
            pitch = System.Math.Atan((v2 - System.Math.Sqrt(discriminant)) / (g * horizontal));
            return true;
        }

        static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;
    }
}
