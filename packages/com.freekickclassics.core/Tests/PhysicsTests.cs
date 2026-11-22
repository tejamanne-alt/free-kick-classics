using System;
using System.Collections.Generic;
using FreeKickClassics.Core.Physics;
using NUnit.Framework;

namespace FreeKickClassics.Core.Tests
{
    public class FlightModelTests
    {
        static readonly EraProfile Vacuum = new EraProfile(Era.Modern, 0.43, 0, 0, 0, 0, 10, 0.1, 0, 0, 0.6);
        static readonly EraProfile NoKnuckle = new EraProfile(Era.Modern, 0.42, 0.22, 0.95, 0.32, 0, 14, 0.12, 0.01, 0.08, 0.65);

        static FlightModel Model(EraProfile era, MatchConditions conditions = null) => new FlightModel(era, conditions ?? MatchConditions.Calm);

        static LaunchParams Shot(Vec3 velocity, Vec3 spin, ulong seed = 42) => new LaunchParams(Pitch.PenaltySpot, velocity, spin, seed);

        [Test]
        public void WithoutAirTheBallIsAPlainProjectile()
        {
            var v0 = new Vec3(1, 5, 20);
            var simulator = new FlightSimulator(Model(Vacuum), Shot(v0, Vec3.Zero));
            for (int i = 0; i < 96; i++) simulator.Step();

            double t = simulator.State.Time;
            var expected = Pitch.PenaltySpot + v0 * t + new Vec3(0, -0.5 * FlightModel.Gravity * t * t, 0);
            Assert.That((simulator.State.Position - expected).Length, Is.LessThan(1e-9));
        }

        [Test]
        public void DragSlowsTheBall()
        {
            var shot = Shot(new Vec3(0, 3, 25), Vec3.Zero);
            var inAir = Flight.Simulate(Model(NoKnuckle), shot, Pitch.PenaltyDistance);
            var inVacuum = Flight.Simulate(Model(Vacuum), shot, Pitch.PenaltyDistance);
            Assert.That(inAir.GoalLine.Time, Is.GreaterThan(inVacuum.GoalLine.Time));
        }

        [Test]
        public void SidespinCurlsTheBallTheWayTheConventionSays()
        {
            var straight = Flight.Simulate(Model(NoKnuckle), Shot(new Vec3(0, 3, 25), Vec3.Zero), Pitch.PenaltyDistance);
            var curlRight = Flight.Simulate(Model(NoKnuckle), Shot(new Vec3(0, 3, 25), new Vec3(0, 40, 0)), Pitch.PenaltyDistance);
            var curlLeft = Flight.Simulate(Model(NoKnuckle), Shot(new Vec3(0, 3, 25), new Vec3(0, -40, 0)), Pitch.PenaltyDistance);
            Assert.That(curlRight.GoalLine.Point.X, Is.GreaterThan(straight.GoalLine.Point.X + 0.3));
            Assert.That(curlLeft.GoalLine.Point.X, Is.LessThan(straight.GoalLine.Point.X - 0.3));
        }

        [Test]
        public void TopspinDipsAndBackspinFloats()
        {
            var velocity = new Vec3(0, 4, 22);
            double plain = Flight.Simulate(Model(NoKnuckle), Shot(velocity, Vec3.Zero), Pitch.PenaltyDistance).GoalLine.Point.Y;
            double topspin = Flight.Simulate(Model(NoKnuckle), Shot(velocity, new Vec3(40, 0, 0)), Pitch.PenaltyDistance).GoalLine.Point.Y;
            double backspin = Flight.Simulate(Model(NoKnuckle), Shot(velocity, new Vec3(-40, 0, 0)), Pitch.PenaltyDistance).GoalLine.Point.Y;
            Assert.That(topspin, Is.LessThan(plain - 0.1));
            Assert.That(backspin, Is.GreaterThan(plain + 0.1));
        }

        [Test]
        public void TheNinetiesBallBendsMoreThanTheLeatherOne()
        {
            // A 25 m free kick with heavy sidespin.
            var launch = new LaunchParams(new Vec3(0, Pitch.BallRadius, 0), new Vec3(-2, 5, 26), new Vec3(0, 60, 0), 1);
            var noSpin = new LaunchParams(launch.Position, launch.Velocity, Vec3.Zero, 1);
            double Bend(EraProfile era) =>
                Flight.Simulate(Model(era), launch, 25).GoalLine.Point.X - Flight.Simulate(Model(era), noSpin, 25).GoalLine.Point.X;

            double seventies = Bend(EraProfile.Seventies);
            double nineties = Bend(EraProfile.Nineties);
            Assert.That(seventies, Is.GreaterThan(0.3));
            Assert.That(nineties, Is.GreaterThan(seventies * 1.5));
        }

        [Test]
        public void ALowSpinModernStrikeKnucklesLate()
        {
            var launch = new LaunchParams(new Vec3(0, Pitch.BallRadius, 0), new Vec3(0, 4, 30), new Vec3(2, 0, 0), 0);
            double maxLateWobble = 0;
            for (ulong seed = 0; seed < 20; seed++)
            {
                var seeded = new LaunchParams(launch.Position, launch.Velocity, launch.Spin, seed);
                var steady = new List<BallState>();
                var knuckle = new List<BallState>();
                Flight.Simulate(Model(NoKnuckle), seeded, 30, trajectory: steady);
                Flight.Simulate(Model(EraProfile.Modern), seeded, 30, trajectory: knuckle);

                // Nothing in the first tenth of a second...
                int early = (int)(0.1 / FlightModel.TimeStep);
                Assert.That((knuckle[early].Position - steady[early].Position).Length, Is.LessThan(1e-9));

                int end = Math.Min(steady.Count, knuckle.Count) - 1;
                maxLateWobble = Math.Max(maxLateWobble, (knuckle[end].Position - steady[end].Position).Length);
            }

            // ...then a visible wobble by the goal line.
            Assert.That(maxLateWobble, Is.GreaterThan(0.25));
        }

        [Test]
        public void SameLaunchGivesTheSameFlightBitForBit()
        {
            var launch = Shot(new Vec3(0.7, 4.1, 28.3), new Vec3(3, -1, 0), 99);
            var first = Flight.Simulate(Model(EraProfile.Modern), launch, Pitch.PenaltyDistance);
            var second = Flight.Simulate(Model(EraProfile.Modern), launch, Pitch.PenaltyDistance);
            Assert.That(second.GoalLine.Point, Is.EqualTo(first.GoalLine.Point));
            Assert.That(second.GoalLine.Time, Is.EqualTo(first.GoalLine.Time));
        }

        [Test]
        public void ThinAirAtAltitudeCarriesTheBallFurther()
        {
            var launch = Shot(new Vec3(0, 8, 24), Vec3.Zero);
            var seaLevel = Flight.Simulate(Model(NoKnuckle), launch, 200);
            var altitude = Flight.Simulate(Model(NoKnuckle, new MatchConditions(Vec3.Zero, 0, 3600)), launch, 200);
            Assert.That(altitude.Final.Position.Z, Is.GreaterThan(seaLevel.Final.Position.Z + 1));
        }

        [Test]
        public void CrosswindPushesTheBall()
        {
            var launch = Shot(new Vec3(0, 3, 25), Vec3.Zero);
            var windy = Flight.Simulate(Model(NoKnuckle, new MatchConditions(new Vec3(8, 0, 0), 0, 0)), launch, Pitch.PenaltyDistance);
            Assert.That(windy.GoalLine.Point.X, Is.GreaterThan(0.02));
        }

        [Test]
        public void TheLeatherBallSoaksUpRain()
        {
            var dry = Model(EraProfile.Seventies);
            var monsoon = Model(EraProfile.Seventies, new MatchConditions(Vec3.Zero, 1, 0));
            Assert.That(monsoon.MassKg / dry.MassKg, Is.EqualTo(1.2).Within(1e-9));
            Assert.That(Model(EraProfile.Modern, new MatchConditions(Vec3.Zero, 1, 0)).MassKg / Model(EraProfile.Modern).MassKg, Is.LessThan(1.02));
        }

        [Test]
        public void ABallDrivenIntoTheGroundBouncesAndNeverSinks()
        {
            var trajectory = new List<BallState>();
            Flight.Simulate(Model(EraProfile.Modern), Shot(new Vec3(0, -6, 20), Vec3.Zero), 30, trajectory: trajectory);
            Assert.That(trajectory.TrueForAll(s => s.Position.Y >= Pitch.BallRadius - 1e-12), Is.True);
            Assert.That(trajectory.Exists(s => s.Velocity.Y > 1), Is.True);
        }

        [Test]
        public void TracksTheWallPlaneOnTheWay()
        {
            var result = Flight.Simulate(Model(EraProfile.Modern), Shot(new Vec3(0, 6, 24), Vec3.Zero), 20, wallZ: Pitch.WallDistance);
            Assert.That(result.Wall.Happened, Is.True);
            Assert.That(result.Wall.Point.Z, Is.EqualTo(Pitch.WallDistance));
            Assert.That(result.Wall.Time, Is.LessThan(result.GoalLine.Time));
        }
    }

    public class GoalFrameTests
    {
        [TestCase(0, 1.2, GoalPlaneZone.OnTarget)]
        [TestCase(-3.5, 0.3, GoalPlaneZone.OnTarget)]
        [TestCase(3.72, 1.0, GoalPlaneZone.Woodwork)]
        [TestCase(-3.60, 1.0, GoalPlaneZone.Woodwork)]
        [TestCase(0, 2.5, GoalPlaneZone.Woodwork)]
        [TestCase(3.75, 2.55, GoalPlaneZone.Woodwork)]
        [TestCase(4.5, 1.0, GoalPlaneZone.OffTarget)]
        [TestCase(0, 2.8, GoalPlaneZone.OffTarget)]
        public void Classifies(double x, double y, GoalPlaneZone expected)
        {
            Assert.That(GoalFrame.Classify(x, y), Is.EqualTo(expected));
        }
    }

    public class AimSolverTests
    {
        [Test]
        public void HitsTargetsAllOverTheGoalInEveryEra([Values(Era.Seventies, Era.Nineties, Era.Modern)] Era era)
        {
            var model = new FlightModel(EraProfile.For(era), MatchConditions.Calm);
            var targets = new[] { (-2.9, 0.3), (0.0, 1.2), (2.9, 2.2), (-4.6, 1.0), (0.4, 3.0), (3.72, 0.8) };
            var strikes = new[] { (22.0, new Vec3(4, 18, 0)), (30.0, new Vec3(3, 0, 0)), (14.5, new Vec3(-22, 0, 0)) };

            foreach (var (x, y) in targets)
            foreach (var (speed, spin) in strikes)
            {
                bool solved = false;
                Vec3 velocity = Vec3.Zero;
                for (int bump = 0; bump <= 10 && !solved; bump++)
                {
                    solved = AimSolver.TrySolve(model, Pitch.PenaltySpot, speed + bump, spin, 17, Pitch.PenaltyDistance, x, y, out velocity);
                }

                Assert.That(solved, Is.True, $"{era}: target ({x}, {y}) at {speed} m/s");
                var crossing = Flight.Simulate(model, new LaunchParams(Pitch.PenaltySpot, velocity, spin, 17), Pitch.PenaltyDistance).GoalLine;
                Assert.That(Math.Abs(crossing.Point.X - x), Is.LessThanOrEqualTo(AimSolver.Tolerance));
                Assert.That(Math.Abs(crossing.Point.Y - y), Is.LessThanOrEqualTo(AimSolver.Tolerance));
            }
        }
    }

    public class WireUnitsTests
    {
        [Test]
        public void QuantizesToTheWireAndBack()
        {
            Assert.That(WireUnits.FromCenti(WireUnits.ToCenti(27.384)), Is.EqualTo(27.38));
            Assert.That(WireUnits.FromCenti(WireUnits.ToCenti(-0.005)), Is.EqualTo(-0.01));
            Assert.That(WireUnits.FromMillimetres(WireUnits.ToMillimetres(11.0004)), Is.EqualTo(11.0));
            var v = new Vec3(1.234567, -2.5, 30.001);
            Assert.That(WireUnits.Quantize(WireUnits.Quantize(v)), Is.EqualTo(WireUnits.Quantize(v)));
        }

        [Test]
        public void RefusesValuesThatDontFit()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => WireUnits.ToCenti(400));
            Assert.Throws<ArgumentOutOfRangeException>(() => WireUnits.ToMillimetres(-40));
            Assert.Throws<ArgumentOutOfRangeException>(() => WireUnits.ToCenti(double.NaN));
        }
    }

    public class TrajectoryReconcilerTests
    {
        [Test]
        public void StartsAtZeroAndEndsExactlyOnTheHostsPoint()
        {
            var local = new Vec3(1.0, 1.0, 11);
            var host = new Vec3(1.03, 0.98, 11);
            Assert.That(TrajectoryReconciler.Correction(local, host, 0, 0.5), Is.EqualTo(Vec3.Zero));
            Assert.That((local + TrajectoryReconciler.Correction(local, host, 0.5, 0.5) - host).Length, Is.LessThan(1e-12));

            double previous = 0;
            for (double t = 0; t <= 0.5; t += 0.05)
            {
                double size = TrajectoryReconciler.Correction(local, host, t, 0.5).Length;
                Assert.That(size, Is.GreaterThanOrEqualTo(previous));
                previous = size;
            }
        }
    }
}
