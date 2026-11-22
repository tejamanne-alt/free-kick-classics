using System;
using System.Collections.Generic;

namespace FreeKickClassics.Core.Physics
{
    /// <summary>
    /// Forces on the ball in flight: gravity, quadratic drag, Magnus lift from spin, and a seeded knuckle wobble for
    /// low-spin strikes, tuned per era and adjusted for wind, rain and altitude.
    /// </summary>
    /// <remarks>
    /// The inner loop uses only +, -, *, / and sqrt, so on most phones the same launch gives the same flight. That
    /// isn't guaranteed across chips and compilers (IL2CPP's C++ compiler may fuse multiply-adds), which is why the host
    /// is the authority on where the ball ends up and the guest nudges its replay onto the host's end point
    /// (<see cref="TrajectoryReconciler"/>).
    /// </remarks>
    public sealed class FlightModel
    {
        public const double TimeStep = 1.0 / 240.0;
        public const double Gravity = 9.81;

        /// <summary>Knuckling builds up over the flight: none before this time, so the wobble comes late.</summary>
        const double KnuckleOnsetSeconds = 0.10;
        const double KnuckleRampSeconds = 0.30;
        const double KnuckleVerticalShare = 0.6;
        /// <summary>Below this impact speed a ball touching the grass rolls instead of bouncing.</summary>
        const double RollThreshold = 0.5;

        readonly double aeroPerMass;
        readonly double spinRetentionPerStep;

        public FlightModel(EraProfile era, MatchConditions conditions)
        {
            Era = era ?? throw new ArgumentNullException(nameof(era));
            Conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));

            MassKg = era.BallMassKg * (1 + era.WaterAbsorption * conditions.Rain);
            double area = System.Math.PI * Pitch.BallRadius * Pitch.BallRadius;
            aeroPerMass = 0.5 * conditions.AirDensity * area / MassKg;
            spinRetentionPerStep = System.Math.Max(0, 1 - era.SpinDecayPerSecond * TimeStep);

            // A wet pitch is slick: the ball keeps more pace when it skids off the surface and rolls further.
            BounceTangentialRetention = 0.72 + 0.18 * conditions.Rain;
            RollingResistance = 0.06 - 0.03 * conditions.Rain;
        }

        public EraProfile Era { get; }

        public MatchConditions Conditions { get; }

        /// <summary>Ball mass after soaking up rain.</summary>
        public double MassKg { get; }

        public double BounceTangentialRetention { get; }

        public double RollingResistance { get; }

        public double SpinRetentionPerStep => spinRetentionPerStep;

        public Vec3 Acceleration(Vec3 velocity, Vec3 spin, double time, ulong knuckleSeed)
        {
            var acceleration = new Vec3(0, -Gravity, 0);
            Vec3 air = velocity - Conditions.Wind;
            double speedSq = air.LengthSquared;
            if (speedSq < 1e-12) return acceleration;
            double speed = System.Math.Sqrt(speedSq);

            acceleration -= air * (aeroPerMass * Era.DragCoefficient * speed);

            double spinRate = spin.Length;
            if (spinRate > 1e-9)
            {
                double spinParameter = Pitch.BallRadius * spinRate / speed;
                double lift = System.Math.Min(Era.LiftSlope * spinParameter, Era.LiftMax);
                // |Cross(spin, air)| = spinRate * speed * sin(angle), so this has magnitude k * lift * speed^2.
                acceleration += Vec3.Cross(spin, air) * (aeroPerMass * lift * speed / spinRate);
            }

            double knuckle = KnuckleWeight(spinRate, time);
            if (knuckle > 0)
            {
                KnuckleNoise(knuckleSeed, time, out double side, out double vertical);
                Basis(air / speed, out Vec3 sideAxis, out Vec3 liftAxis);
                Vec3 direction = sideAxis * side + liftAxis * (vertical * KnuckleVerticalShare);
                acceleration += direction * (aeroPerMass * Era.KnuckleCoefficient * knuckle * speedSq);
            }

            return acceleration;
        }

        /// <summary>Resolves contact with the grass after a step that ended below ball height.</summary>
        public void ResolveGround(ref Vec3 position, ref Vec3 velocity, ref Vec3 spin)
        {
            if (position.Y >= Pitch.BallRadius) return;
            double impactSpeed = -velocity.Y;
            position = new Vec3(position.X, Pitch.BallRadius, position.Z);

            if (impactSpeed > RollThreshold)
            {
                double keep = BounceTangentialRetention;
                velocity = new Vec3(velocity.X * keep, impactSpeed * Era.Restitution, velocity.Z * keep);
                spin = spin * 0.6;
                return;
            }

            var horizontal = new Vec3(velocity.X, 0, velocity.Z);
            double horizontalSpeed = horizontal.Length;
            double slowdown = RollingResistance * Gravity * TimeStep;
            velocity = horizontalSpeed <= slowdown ? Vec3.Zero : horizontal * (1 - slowdown / horizontalSpeed);
        }

        double KnuckleWeight(double spinRate, double time)
        {
            if (Era.KnuckleCoefficient <= 0) return 0;
            double lowSpin = 1 - spinRate / Era.KnuckleSpinThreshold;
            if (lowSpin <= 0) return 0;
            double ramp = (time - KnuckleOnsetSeconds) / KnuckleRampSeconds;
            if (ramp <= 0) return 0;
            if (ramp > 1) ramp = 1;
            return lowSpin * lowSpin * ramp;
        }

        /// <summary>Smoothly drifting 2D noise in [-1, 1], the same on every phone for a given seed.</summary>
        void KnuckleNoise(ulong seed, double time, out double side, out double vertical)
        {
            double position = time / Era.KnuckleSegmentSeconds;
            double segment = System.Math.Floor(position);
            double t = position - segment;
            double smooth = t * t * (3 - 2 * t);
            ulong index = (ulong)(long)segment;

            ulong a = DeterministicRandom.Hash(seed, index);
            ulong b = DeterministicRandom.Hash(seed, index + 1);
            side = Lerp(Unit(a), Unit(b), smooth);
            vertical = Lerp(Unit(a >> 32), Unit(b >> 32), smooth);
        }

        static double Unit(ulong bits) => (bits & 0xFFFFFFFFUL) * (2.0 / 4294967295.0) - 1.0;

        static double Lerp(double a, double b, double t) => a + (b - a) * t;

        static void Basis(Vec3 direction, out Vec3 side, out Vec3 lift)
        {
            Vec3 cross = Vec3.Cross(Vec3.Up, direction);
            double length = cross.Length;
            side = length > 1e-6 ? cross / length : Vec3.Right;
            lift = Vec3.Cross(direction, side);
        }
    }

    public struct BallState
    {
        public Vec3 Position;
        public Vec3 Velocity;
        public Vec3 Spin;
        public double Time;
    }

    /// <summary>Steps one ball through a flight at the fixed <see cref="FlightModel.TimeStep"/>.</summary>
    public sealed class FlightSimulator
    {
        readonly FlightModel model;
        readonly ulong knuckleSeed;
        readonly bool withGround;
        BallState state;

        /// <param name="withGround">False lets the ball fly through the grass, which the aim solver needs.</param>
        public FlightSimulator(FlightModel model, LaunchParams launch, bool withGround = true)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.withGround = withGround;
            knuckleSeed = launch.KnuckleSeed;
            state = new BallState { Position = launch.Position, Velocity = launch.Velocity, Spin = launch.Spin, Time = 0 };
        }

        public BallState State => state;

        /// <summary>One explicit-midpoint step.</summary>
        public void Step()
        {
            const double dt = FlightModel.TimeStep;
            Vec3 v = state.Velocity;
            Vec3 spin = state.Spin;
            double t = state.Time;

            Vec3 a1 = model.Acceleration(v, spin, t, knuckleSeed);
            Vec3 vMid = v + a1 * (dt * 0.5);
            Vec3 a2 = model.Acceleration(vMid, spin, t + dt * 0.5, knuckleSeed);

            Vec3 position = state.Position + vMid * dt;
            Vec3 velocity = v + a2 * dt;
            spin = spin * model.SpinRetentionPerStep;
            if (withGround) model.ResolveGround(ref position, ref velocity, ref spin);

            state.Position = position;
            state.Velocity = velocity;
            state.Spin = spin;
            state.Time = t + dt;
        }
    }

    public readonly struct PlaneCrossing
    {
        public readonly bool Happened;
        public readonly Vec3 Point;
        public readonly double Time;

        public PlaneCrossing(Vec3 point, double time)
        {
            Happened = true;
            Point = point;
            Time = time;
        }
    }

    public readonly struct FlightResult
    {
        /// <summary>Where the ball's centre crossed the goal-line plane, if it got there.</summary>
        public readonly PlaneCrossing GoalLine;
        /// <summary>Where it crossed the wall's plane, if a wall distance was given.</summary>
        public readonly PlaneCrossing Wall;
        public readonly BallState Final;

        public FlightResult(PlaneCrossing goalLine, PlaneCrossing wall, BallState final)
        {
            GoalLine = goalLine;
            Wall = wall;
            Final = final;
        }

        /// <summary>The goal-line crossing if there was one, otherwise where the ball stopped being simulated.</summary>
        public Vec3 EndPoint => GoalLine.Happened ? GoalLine.Point : Final.Position;

        public double EndTime => GoalLine.Happened ? GoalLine.Time : Final.Time;
    }

    public static class Flight
    {
        public const double DefaultMaxSeconds = 4.0;

        /// <summary>
        /// Flies the ball until its centre crosses the goal-line plane, it stops, it goes far out of play, or
        /// <paramref name="maxSeconds"/> pass. Collisions with the wall, keeper and posts belong to the engine.
        /// </summary>
        /// <param name="wallZ">Plane of the wall, or NaN for none.</param>
        /// <param name="trajectory">If given, receives the launch state and every step up to the end.</param>
        /// <param name="withGround">False lets the ball fly through the grass (for aiming).</param>
        public static FlightResult Simulate(
            FlightModel model,
            LaunchParams launch,
            double goalLineZ,
            double wallZ = double.NaN,
            double maxSeconds = DefaultMaxSeconds,
            List<BallState> trajectory = null,
            bool withGround = true)
        {
            var simulator = new FlightSimulator(model, launch, withGround);
            trajectory?.Add(simulator.State);
            var goal = default(PlaneCrossing);
            var wall = default(PlaneCrossing);
            bool trackWall = !double.IsNaN(wallZ);

            while (simulator.State.Time < maxSeconds)
            {
                BallState before = simulator.State;
                simulator.Step();
                BallState after = simulator.State;
                trajectory?.Add(after);

                if (trackWall && !wall.Happened && TryCross(before, after, wallZ, out var wallCrossing)) wall = wallCrossing;

                if (TryCross(before, after, goalLineZ, out goal)) break;

                bool stopped = withGround && after.Position.Y <= Pitch.BallRadius && after.Velocity.LengthSquared < 0.0025;
                bool goneBackwards = after.Position.Z < launch.Position.Z - 5;
                bool goneWide = System.Math.Abs(after.Position.X - launch.Position.X) > 60;
                if (stopped || goneBackwards || goneWide) break;
            }

            return new FlightResult(goal, wall, simulator.State);
        }

        static bool TryCross(BallState before, BallState after, double planeZ, out PlaneCrossing crossing)
        {
            crossing = default;
            if (!(before.Position.Z < planeZ && after.Position.Z >= planeZ)) return false;
            double f = (planeZ - before.Position.Z) / (after.Position.Z - before.Position.Z);
            Vec3 point = Vec3.Lerp(before.Position, after.Position, f);
            crossing = new PlaneCrossing(new Vec3(point.X, point.Y, planeZ), before.Time + (after.Time - before.Time) * f);
            return true;
        }
    }

    /// <summary>
    /// Lets the guest's replay finish exactly where the host said the ball went, even if the guest's chip computed a
    /// slightly different flight. The correction starts at zero and grows smoothly, so the strike itself looks the same.
    /// </summary>
    public static class TrajectoryReconciler
    {
        /// <returns>The offset to add to the guest's simulated position at <paramref name="time"/>.</returns>
        public static Vec3 Correction(Vec3 localEnd, Vec3 hostEnd, double time, double endTime)
        {
            Vec3 total = hostEnd - localEnd;
            if (endTime <= 0) return total;
            double f = time / endTime;
            if (f <= 0) return Vec3.Zero;
            if (f >= 1) return total;
            return total * (f * f * (3 - 2 * f));
        }
    }
}
