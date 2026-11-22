using System;
using FreeKickClassics.Core.Physics;

namespace FreeKickClassics.Core.Shootout
{
    /// <summary>A resolved penalty turned into a flight both phones can replay.</summary>
    public readonly struct PlannedKick
    {
        /// <summary>Already quantized to what the wire carries.</summary>
        public readonly LaunchParams Launch;
        /// <summary>Where the host's simulation has the ball's centre cross the goal-line plane.</summary>
        public readonly Vec3 EndPoint;
        public readonly double FlightSeconds;

        public PlannedKick(LaunchParams launch, Vec3 endPoint, double flightSeconds)
        {
            Launch = launch;
            EndPoint = endPoint;
            FlightSeconds = flightSeconds;
        }
    }

    /// <summary>Runs on the host: chooses the launch that sends the ball where the resolver said it goes.</summary>
    public interface IKickPlanner
    {
        PlannedKick Plan(KickerChoice choice, ShotResolution resolution, ulong knuckleSeed);
    }

    public sealed class PenaltyKickPlanner : IKickPlanner
    {
        const int MaxSpeedBumps = 10;

        /// <summary>How close the re-flown launch must land to the target, in metres.</summary>
        public const double LandingTolerance = 0.01;

        readonly FlightModel model;

        public PenaltyKickPlanner(FlightModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public FlightModel Model => model;

        /// <summary>Ball speed off the boot in m/s for a strike type and power.</summary>
        public static double ShotSpeed(KickerChoice choice)
        {
            double power = choice.Power / 255.0;
            switch (choice.Strike)
            {
                case StrikeType.Placed: return 17 + 6 * power;
                case StrikeType.Driven: return 24 + 8 * power;
                case StrikeType.Chip: return 13 + 3 * power;
                default: throw new ArgumentOutOfRangeException(nameof(choice));
            }
        }

        /// <summary>
        /// Spin by strike type: a placed shot curls away from the middle towards its corner, a driven shot carries
        /// almost no spin (so a modern ball knuckles), and a chip has backspin.
        /// </summary>
        public static Vec3 ShotSpin(KickerChoice choice)
        {
            switch (choice.Strike)
            {
                case StrikeType.Placed:
                    double curl = choice.Column == GoalColumn.Left ? -18 : choice.Column == GoalColumn.Right ? 18 : 0;
                    return new Vec3(4, curl, 0);
                case StrikeType.Driven:
                    return new Vec3(3, 0, 0);
                case StrikeType.Chip:
                    return new Vec3(-22, 0, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice));
            }
        }

        public PlannedKick Plan(KickerChoice choice, ShotResolution resolution, ulong knuckleSeed)
        {
            Vec3 start = Pitch.PenaltySpot;
            double targetX = resolution.TargetXmm / 1000.0;
            double targetY = resolution.TargetYmm / 1000.0;
            Vec3 spin = ShotSpin(choice);
            // The same swing moves a waterlogged ball less: scale by momentum.
            double speed = ShotSpeed(choice) * model.Era.BallMassKg / model.MassKg;

            for (int bump = 0; bump <= MaxSpeedBumps; bump++)
            {
                if (!AimSolver.TrySolve(model, start, speed + bump, spin, knuckleSeed, Pitch.PenaltyDistance, targetX, targetY, out Vec3 velocity))
                {
                    continue;
                }

                // Re-fly what will actually be sent, grass included, and only accept it if it still lands on target.
                var launch = new LaunchParams(start, velocity, spin, knuckleSeed).Quantized();
                var flight = Flight.Simulate(model, launch, Pitch.PenaltyDistance);
                if (flight.GoalLine.Happened &&
                    System.Math.Abs(flight.GoalLine.Point.X - targetX) <= LandingTolerance &&
                    System.Math.Abs(flight.GoalLine.Point.Y - targetY) <= LandingTolerance)
                {
                    return new PlannedKick(launch, flight.EndPoint, flight.EndTime);
                }
            }

            throw new InvalidOperationException("No launch reaches " + resolution + " with " + choice + ".");
        }
    }
}
