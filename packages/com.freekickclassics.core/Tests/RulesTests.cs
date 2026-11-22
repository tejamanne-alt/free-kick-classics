using System;
using System.Collections.Generic;
using System.Linq;
using FreeKickClassics.Core.Physics;
using FreeKickClassics.Core.Shootout;
using FreeKickClassics.DuelLink;
using NUnit.Framework;

namespace FreeKickClassics.Core.Tests
{
    static class Picks
    {
        public static IEnumerable<KickerChoice> AllKicks(params byte[] powers)
        {
            if (powers.Length == 0) powers = new byte[] { 0, 64, 160, 240, 255 };
            foreach (GoalRow row in Enum.GetValues(typeof(GoalRow)))
            foreach (GoalColumn column in Enum.GetValues(typeof(GoalColumn)))
            foreach (StrikeType strike in Enum.GetValues(typeof(StrikeType)))
            foreach (byte power in powers)
            foreach (bool stutter in new[] { false, true })
            {
                yield return new KickerChoice(column, row, strike, power, stutter);
            }
        }

        public static IEnumerable<KeeperChoice> AllSaves()
        {
            foreach (DiveSide side in Enum.GetValues(typeof(DiveSide)))
            foreach (DiveTiming timing in Enum.GetValues(typeof(DiveTiming)))
            foreach (bool shuffle in new[] { false, true })
            {
                yield return new KeeperChoice(side, timing, shuffle);
            }
        }

        public static KickerChoice RandomKick(DeterministicRandom random) =>
            new KickerChoice((GoalColumn)random.NextInt(3), (GoalRow)random.NextInt(3), (StrikeType)random.NextInt(3), (byte)random.NextInt(256), random.NextInt(5) == 0);

        public static KeeperChoice RandomSave(DeterministicRandom random) =>
            new KeeperChoice((DiveSide)random.NextInt(2), (DiveTiming)random.NextInt(3), random.NextInt(4) == 0);
    }

    public class ChoicesTests
    {
        [Test]
        public void EveryPickRoundTripsThroughFourBytes()
        {
            foreach (var kick in Picks.AllKicks())
            {
                Assert.That(kick.Encode().Length, Is.EqualTo(4));
                Assert.That(KickerChoice.TryDecode(kick.Encode(), out var decoded), Is.True);
                Assert.That(decoded, Is.EqualTo(kick));
            }

            foreach (var save in Picks.AllSaves())
            {
                Assert.That(KeeperChoice.TryDecode(save.Encode(), out var decoded), Is.True);
                Assert.That(decoded, Is.EqualTo(save));
            }
        }

        [Test]
        public void RejectsOutOfRangeOrUnknownBits()
        {
            Assert.That(KickerChoice.TryDecode(new byte[] { 9, 0, 0, 0 }, out _), Is.False);
            Assert.That(KickerChoice.TryDecode(new byte[] { 0, 3, 0, 0 }, out _), Is.False);
            Assert.That(KickerChoice.TryDecode(new byte[] { 0, 0, 0, 2 }, out _), Is.False);
            Assert.That(KickerChoice.TryDecode(new byte[] { 0, 0, 0 }, out _), Is.False);
            Assert.That(KeeperChoice.TryDecode(new byte[] { 2, 0, 0, 0 }, out _), Is.False);
            Assert.That(KeeperChoice.TryDecode(new byte[] { 0, 3, 0, 0 }, out _), Is.False);
            Assert.That(KeeperChoice.TryDecode(new byte[] { 0, 0, 1, 0 }, out _), Is.False);
        }
    }

    public class PenaltyResolverTests
    {
        readonly PenaltyResolver resolver = new PenaltyResolver(ShootoutTuning.CreateDefault());

        [Test]
        public void SameSeedSameResult()
        {
            var kick = new KickerChoice(GoalColumn.Left, GoalRow.High, StrikeType.Driven, 230);
            var save = new KeeperChoice(DiveSide.Left, DiveTiming.Late);
            for (ulong seed = 0; seed < 200; seed++)
            {
                Assert.That(resolver.Resolve(kick, save, seed), Is.EqualTo(resolver.Resolve(kick, save, seed)));
            }
        }

        [Test]
        public void WhereTheBallGoesMatchesWhatHappened()
        {
            var random = new DeterministicRandom(5);
            var seen = new HashSet<ShotOutcome>();
            for (int i = 0; i < 20000; i++)
            {
                var kick = Picks.RandomKick(random);
                var save = Picks.RandomSave(random);
                var resolution = resolver.Resolve(kick, save, random.NextULong());
                seen.Add(resolution.Outcome);

                var zone = GoalFrame.Classify(resolution.TargetXmm / 1000.0, resolution.TargetYmm / 1000.0);
                var expected = resolution.Outcome == ShotOutcome.Missed ? GoalPlaneZone.OffTarget
                    : resolution.Outcome == ShotOutcome.Woodwork ? GoalPlaneZone.Woodwork
                    : GoalPlaneZone.OnTarget;
                Assert.That(zone, Is.EqualTo(expected), $"{kick} vs {save}: {resolution}");
            }

            Assert.That(seen.Count, Is.EqualTo(4));
        }

        [Test]
        public void ResolveFollowsTheStatedProbabilities()
        {
            var kick = new KickerChoice(GoalColumn.Right, GoalRow.Low, StrikeType.Placed, 160);
            var save = new KeeperChoice(DiveSide.Right, DiveTiming.Late);
            int goals = 0;
            const int trials = 20000;
            for (ulong seed = 0; seed < trials; seed++)
            {
                if (resolver.Resolve(kick, save, seed * 7919).IsGoal) goals++;
            }

            Assert.That(goals / (double)trials, Is.EqualTo(resolver.GoalProbability(kick, save)).Within(0.015));
        }

        [Test]
        public void TheFootballLogicHolds()
        {
            var lowLeft = new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 160);
            var centreChip = new KickerChoice(GoalColumn.Centre, GoalRow.Middle, StrikeType.Chip, 100);
            var stutterLeft = new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 160, stutter: true);

            var earlyLeft = new KeeperChoice(DiveSide.Left, DiveTiming.Early);
            var earlyRight = new KeeperChoice(DiveSide.Right, DiveTiming.Early);
            var lateLeft = new KeeperChoice(DiveSide.Left, DiveTiming.Late);
            var stayBig = new KeeperChoice(DiveSide.Left, DiveTiming.StayBig);

            // Diving the wrong way early leaves the corner empty.
            Assert.That(resolver.SavePerMille(lowLeft, earlyRight), Is.EqualTo(0));
            // Guessing right early beats reacting late.
            Assert.That(resolver.SavePerMille(lowLeft, earlyLeft), Is.GreaterThan(resolver.SavePerMille(lowLeft, lateLeft)));
            // The chip humiliates an early dive and gets caught by a keeper who stays big.
            Assert.That(resolver.SavePerMille(centreChip, earlyLeft), Is.LessThan(20));
            Assert.That(resolver.SavePerMille(centreChip, stayBig), Is.GreaterThan(900));
            // A stutter wrecks an early dive but costs accuracy, and does nothing to a late one.
            Assert.That(resolver.SavePerMille(stutterLeft, earlyLeft), Is.LessThan(resolver.SavePerMille(lowLeft, earlyLeft) / 2));
            Assert.That(resolver.MissPerMille(stutterLeft, earlyLeft), Is.GreaterThan(resolver.MissPerMille(lowLeft, earlyLeft)));
            Assert.That(resolver.SavePerMille(stutterLeft, lateLeft), Is.EqualTo(resolver.SavePerMille(lowLeft, lateLeft)));
            // Shuffling shrinks the sweet spot.
            var shuffle = new KeeperChoice(DiveSide.Left, DiveTiming.Late, shuffle: true);
            Assert.That(resolver.MissPerMille(lowLeft, shuffle), Is.GreaterThan(resolver.MissPerMille(lowLeft, lateLeft)));
            // The low and middle centre never miss.
            Assert.That(resolver.MissPerMille(new KickerChoice(GoalColumn.Centre, GoalRow.Low, StrikeType.Driven, 255, true), shuffle), Is.EqualTo(0));
        }

        [Test]
        public void EveryChanceIsAValidProbability()
        {
            foreach (var kick in Picks.AllKicks())
            foreach (var save in Picks.AllSaves())
            {
                Assert.That(resolver.MissPerMille(kick, save), Is.InRange(0, 1000));
                Assert.That(resolver.SavePerMille(kick, save), Is.InRange(0, 1000));
            }
        }

        [Test]
        public void FingerprintChangesWithAnyNumber()
        {
            var a = ShootoutTuning.CreateDefault();
            var b = ShootoutTuning.CreateDefault();
            Assert.That(b.Fingerprint(), Is.EqualTo(a.Fingerprint()));
            b.CoveragePerMille[2, 1, 1] += 1;
            Assert.That(b.Fingerprint(), Is.Not.EqualTo(a.Fingerprint()));
        }
    }

    /// <summary>
    /// Guards the design, not the code: solves the kicker-vs-keeper game (fictitious play) and checks the defaults
    /// still give a real mind game.
    /// </summary>
    public class PenaltyBalanceTests
    {
        [Test]
        public void EveryOptionIsWorthPickingAndTheGoalRateIsRealistic()
        {
            var resolver = new PenaltyResolver(ShootoutTuning.CreateDefault());
            var kicks = Picks.AllKicks(64, 160, 240).ToArray();
            var saves = Picks.AllSaves().ToArray();
            var payoff = new double[kicks.Length, saves.Length];
            for (int i = 0; i < kicks.Length; i++)
            for (int j = 0; j < saves.Length; j++)
            {
                payoff[i, j] = resolver.GoalProbability(kicks[i], saves[j]);
            }

            Solve(payoff, 150000, out double[] kickMix, out double[] saveMix, out double lower, out double upper);
            double value = (lower + upper) / 2;
            TestContext.WriteLine($"Equilibrium goal rate {value:P1} (bounds {lower:F4}-{upper:F4})");

            Assert.That(upper - lower, Is.LessThan(0.01), "solver converged");
            Assert.That(value, Is.InRange(0.70, 0.80), "close to real penalty conversion (about 75%)");

            // Kicker: the best kick with each feature, against the keeper's optimal mix, is within 3 points of optimal.
            double BestKick(Func<KickerChoice, bool> has)
            {
                double best = 0;
                for (int i = 0; i < kicks.Length; i++)
                {
                    if (!has(kicks[i])) continue;
                    double sum = 0;
                    for (int j = 0; j < saves.Length; j++) sum += saveMix[j] * payoff[i, j];
                    best = Math.Max(best, sum);
                }

                return best;
            }

            double BestSave(Func<KeeperChoice, bool> has)
            {
                double best = 1;
                for (int j = 0; j < saves.Length; j++)
                {
                    if (!has(saves[j])) continue;
                    double sum = 0;
                    for (int i = 0; i < kicks.Length; i++) sum += kickMix[i] * payoff[i, j];
                    best = Math.Min(best, sum);
                }

                return best;
            }

            var regrets = new List<(string Option, double Points)>();
            foreach (GoalRow row in Enum.GetValues(typeof(GoalRow))) regrets.Add(("row " + row, value - BestKick(k => k.Row == row)));
            foreach (GoalColumn column in Enum.GetValues(typeof(GoalColumn))) regrets.Add(("column " + column, value - BestKick(k => k.Column == column)));
            foreach (StrikeType strike in Enum.GetValues(typeof(StrikeType))) regrets.Add((strike.ToString(), value - BestKick(k => k.Strike == strike)));
            foreach (byte power in new byte[] { 64, 160, 240 }) regrets.Add(("power " + power, value - BestKick(k => k.Power == power)));
            regrets.Add(("stutter", value - BestKick(k => k.Stutter)));
            foreach (DiveTiming timing in Enum.GetValues(typeof(DiveTiming))) regrets.Add((timing.ToString(), BestSave(s => s.Timing == timing) - value));
            regrets.Add(("shuffle", BestSave(s => s.Shuffle) - value));

            foreach (var (option, points) in regrets) TestContext.WriteLine($"  {option}: {points * 100:F2} points from optimal");
            foreach (var (option, points) in regrets) Assert.That(points, Is.LessThan(0.03), option);
        }

        /// <summary>Fictitious play for a zero-sum game where the row player maximises.</summary>
        static void Solve(double[,] payoff, int iterations, out double[] rowMix, out double[] columnMix, out double lower, out double upper)
        {
            int rows = payoff.GetLength(0);
            int columns = payoff.GetLength(1);
            var rowTotals = new double[rows];
            var columnTotals = new double[columns];
            var rowCounts = new int[rows];
            var columnCounts = new int[columns];
            int r = 0, c = 0;

            for (int t = 0; t < iterations; t++)
            {
                rowCounts[r]++;
                columnCounts[c]++;
                for (int j = 0; j < columns; j++) columnTotals[j] += payoff[r, j];
                for (int i = 0; i < rows; i++) rowTotals[i] += payoff[i, c];
                r = ArgBest(rowTotals, max: true);
                c = ArgBest(columnTotals, max: false);
            }

            rowMix = rowCounts.Select(n => n / (double)iterations).ToArray();
            columnMix = columnCounts.Select(n => n / (double)iterations).ToArray();
            lower = double.MaxValue;
            upper = double.MinValue;
            for (int j = 0; j < columns; j++)
            {
                double sum = 0;
                for (int i = 0; i < rows; i++) sum += rowMix[i] * payoff[i, j];
                lower = Math.Min(lower, sum);
            }

            for (int i = 0; i < rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < columns; j++) sum += columnMix[j] * payoff[i, j];
                upper = Math.Max(upper, sum);
            }
        }

        static int ArgBest(double[] values, bool max)
        {
            int best = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (max ? values[i] > values[best] : values[i] < values[best]) best = i;
            }

            return best;
        }
    }

    public class ShootoutScoreTests
    {
        [Test]
        public void AlternatesStartingWithTheFirstKicker()
        {
            var score = new ShootoutScore(DuelRole.Guest);
            Assert.That(score.NextKicker, Is.EqualTo(DuelRole.Guest));
            score.Record(DuelRole.Guest, true);
            Assert.That(score.NextKicker, Is.EqualTo(DuelRole.Host));
            Assert.Throws<InvalidOperationException>(() => score.Record(DuelRole.Guest, true));
        }

        [Test]
        public void EndsAsSoonAsOneSideCantCatchUp()
        {
            var score = new ShootoutScore(DuelRole.Host);
            for (int i = 0; i < 3; i++)
            {
                score.Record(DuelRole.Host, true);
                Assert.That(score.IsDecided, Is.False);
                score.Record(DuelRole.Guest, false);
            }

            // 3-0 with two kicks left each: the guest can reach 2 at most.
            Assert.That(score.IsDecided, Is.True);
            Assert.That(score.Winner, Is.EqualTo(DuelRole.Host));
            Assert.Throws<InvalidOperationException>(() => score.Record(DuelRole.Host, true));
        }

        [Test]
        public void CanEndMidPairWhenTheSecondKickerCantCatchUp()
        {
            var score = new ShootoutScore(DuelRole.Host);
            var kicks = new[] { true, false, true, false, true, true, true };
            for (int i = 0; i < kicks.Length; i++) score.Record(i % 2 == 0 ? DuelRole.Host : DuelRole.Guest, kicks[i]);

            // Host 4 (of 4), guest 1 (of 3) with two kicks left: 1 + 2 < 4.
            Assert.That(score.IsDecided, Is.True);
            Assert.That(score.Winner, Is.EqualTo(DuelRole.Host));
        }

        [Test]
        public void ATieGoesToSuddenDeathInPairs()
        {
            var score = new ShootoutScore(DuelRole.Host, kicksPerSide: 2);
            score.Record(DuelRole.Host, true);
            score.Record(DuelRole.Guest, true);
            score.Record(DuelRole.Host, false);
            score.Record(DuelRole.Guest, false);
            Assert.That(score.InSuddenDeath, Is.True);

            score.Record(DuelRole.Host, true);
            Assert.That(score.IsDecided, Is.False, "the guest still gets to answer");
            score.Record(DuelRole.Guest, true);
            Assert.That(score.IsDecided, Is.False);

            score.Record(DuelRole.Host, false);
            score.Record(DuelRole.Guest, true);
            Assert.That(score.IsDecided, Is.True);
            Assert.That(score.Winner, Is.EqualTo(DuelRole.Guest));
            Assert.That(score.Goals(DuelRole.Guest), Is.EqualTo(3));
        }
    }

    public class TendencyTests
    {
        [Test]
        public void SpotsWentLeftFourOfFive()
        {
            var history = new List<KickerChoice>
            {
                new KickerChoice(GoalColumn.Right, GoalRow.High, StrikeType.Driven, 200), // outside the window
                new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 100),
                new KickerChoice(GoalColumn.Left, GoalRow.High, StrikeType.Driven, 100),
                new KickerChoice(GoalColumn.Centre, GoalRow.Middle, StrikeType.Chip, 100),
                new KickerChoice(GoalColumn.Left, GoalRow.Middle, StrikeType.Placed, 100),
                new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Driven, 100),
            };

            Assert.That(TendencyReader.TryGetStrongest(history, out var tendency), Is.True);
            Assert.That(tendency.Kind, Is.EqualTo(TendencyKind.Column));
            Assert.That((GoalColumn)tendency.Value, Is.EqualTo(GoalColumn.Left));
            Assert.That(tendency.Count, Is.EqualTo(4));
            Assert.That(tendency.OutOf, Is.EqualTo(5));
        }

        [Test]
        public void StaysQuietWithoutEnoughEvidence()
        {
            var twoKicks = new List<KickerChoice>
            {
                new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 100),
                new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 100),
            };
            Assert.That(TendencyReader.TryGetStrongest(twoKicks, out _), Is.False);

            var mixed = new List<KickerChoice>
            {
                new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 100),
                new KickerChoice(GoalColumn.Right, GoalRow.Middle, StrikeType.Driven, 100),
                new KickerChoice(GoalColumn.Centre, GoalRow.High, StrikeType.Chip, 100),
            };
            Assert.That(TendencyReader.TryGetStrongest(mixed, out _), Is.False);
        }

        [Test]
        public void ReadsKeeperHabits()
        {
            var history = new List<KeeperChoice>
            {
                new KeeperChoice(DiveSide.Left, DiveTiming.Early),
                new KeeperChoice(DiveSide.Right, DiveTiming.Early),
                new KeeperChoice(DiveSide.Left, DiveTiming.Early),
                new KeeperChoice(DiveSide.Right, DiveTiming.Early),
            };
            Assert.That(TendencyReader.TryGetStrongest(history, out var tendency), Is.True);
            Assert.That(tendency.Kind, Is.EqualTo(TendencyKind.DiveTiming));
            Assert.That((DiveTiming)tendency.Value, Is.EqualTo(DiveTiming.Early));
            Assert.That(tendency.Share, Is.EqualTo(1.0));
        }
    }

    public class CpuKeeperTests
    {
        readonly PenaltyResolver resolver = new PenaltyResolver(ShootoutTuning.CreateDefault());

        [Test]
        public void LearnsAKickerWhoAlwaysGoesLowLeft()
        {
            var keeper = new CpuKeeper(resolver, CpuKeeperProfile.Normal);
            var random = new DeterministicRandom(3);
            var kick = new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 160);
            var history = new List<KickerChoice>();
            int leftDives = 0;
            int rounds = 0;

            for (int i = 0; i < 40; i++)
            {
                var save = keeper.Choose(history, random);
                if (i >= 6)
                {
                    rounds++;
                    if (save.Side == DiveSide.Left && save.Timing != DiveTiming.StayBig) leftDives++;
                }

                history.Add(kick);
            }

            Assert.That(leftDives / (double)rounds, Is.GreaterThan(0.75));
        }

        [Test]
        public void MixesItUpBeforeItHasSeenAnything()
        {
            var keeper = new CpuKeeper(resolver, CpuKeeperProfile.Normal);
            var random = new DeterministicRandom(11);
            var picks = new HashSet<KeeperChoice>();
            for (int i = 0; i < 200; i++) picks.Add(keeper.Choose(new List<KickerChoice>(), random));
            Assert.That(picks.Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(picks.Any(p => p.Side == DiveSide.Left) && picks.Any(p => p.Side == DiveSide.Right), Is.True);
        }

        [Test]
        public void ScoresEveryOption()
        {
            var keeper = new CpuKeeper(resolver, CpuKeeperProfile.Hard);
            var expected = keeper.ExpectedGoalProbabilities(new List<KickerChoice>());
            Assert.That(expected.Length, Is.EqualTo(CpuKeeper.AllOptions.Count));
            Assert.That(expected.All(p => p > 0 && p < 1), Is.True);
        }
    }

    public class PenaltyKickPlannerTests
    {
        [Test]
        public void EveryResolvedKickFliesWhereTheRulesSaid([Values(Era.Seventies, Era.Nineties, Era.Modern)] Era era)
        {
            var resolver = new PenaltyResolver(ShootoutTuning.CreateDefault());
            var conditions = era == Era.Seventies ? new MatchConditions(new Vec3(-3, 0, 2), 1, 0) : MatchConditions.Calm;
            var planner = new PenaltyKickPlanner(new FlightModel(EraProfile.For(era), conditions));
            var random = new DeterministicRandom((ulong)era + 100);

            for (int i = 0; i < 150; i++)
            {
                var kick = Picks.RandomKick(random);
                var resolution = resolver.Resolve(kick, Picks.RandomSave(random), random.NextULong());
                var planned = planner.Plan(kick, resolution, random.NextULong());

                Assert.That(Math.Abs(planned.EndPoint.X - resolution.TargetXmm / 1000.0), Is.LessThan(0.01), $"{kick}: {resolution}");
                Assert.That(Math.Abs(planned.EndPoint.Y - resolution.TargetYmm / 1000.0), Is.LessThan(0.01), $"{kick}: {resolution}");
                Assert.That(planned.EndPoint.Z, Is.EqualTo(Pitch.PenaltyDistance));
                Assert.That(planned.Launch.Velocity, Is.EqualTo(WireUnits.Quantize(planned.Launch.Velocity)));
            }
        }

        [Test]
        public void ReachesEveryCornerOfEveryZoneForEveryStrike([Values(Era.Seventies, Era.Nineties, Era.Modern)] Era era)
        {
            // Sweeps the extremes of each zone's scatter box, including balls skimming the grass, which once tripped
            // the aim solver (a low, central placed shot at 22 cm).
            var tuning = ShootoutTuning.CreateDefault();
            var planner = new PenaltyKickPlanner(new FlightModel(EraProfile.For(era), MatchConditions.Calm));
            int planned = 0;
            foreach (var kick in Picks.AllKicks(0, 126, 255))
            {
                if (kick.Stutter) continue;
                int strike = (int)kick.Strike;
                int cx = tuning.ColumnCentreXmm[(int)kick.Column];
                int cy = tuning.RowCentreYmm[(int)kick.Row];
                foreach (int dx in new[] { -tuning.ScatterXmm[strike], 0, tuning.ScatterXmm[strike] })
                foreach (int dy in new[] { -tuning.ScatterYmm[strike], 0, tuning.ScatterYmm[strike] })
                {
                    int x = Math.Max(-3530, Math.Min(3530, cx + dx));
                    int y = Math.Max(130, Math.Min(2310, cy + dy));
                    var resolution = new ShotResolution(ShotOutcome.Goal, x, y, 0, 0);
                    Assert.DoesNotThrow(() => planner.Plan(kick, resolution, 77), $"{era} {kick} to ({x}, {y})");
                    planned++;
                }
            }

            Assert.That(planned, Is.EqualTo(9 * 3 * 3 * 9));
        }

        [Test]
        public void ReachesTheLowCentralShotThatOnceFailed()
        {
            var planner = new PenaltyKickPlanner(new FlightModel(EraProfile.Modern, MatchConditions.Calm));
            var kick = new KickerChoice(GoalColumn.Centre, GoalRow.Low, StrikeType.Placed, 126);
            var planned = planner.Plan(kick, new ShotResolution(ShotOutcome.Goal, 62, 225, 0, 0), 1);
            Assert.That(Math.Abs(planned.EndPoint.Y - 0.225), Is.LessThan(PenaltyKickPlanner.LandingTolerance));
        }

        [Test]
        public void DrivenShotsAreFasterThanPlacedOnesAndChipsAreSlowest()
        {
            var placed = PenaltyKickPlanner.ShotSpeed(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 128));
            var driven = PenaltyKickPlanner.ShotSpeed(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Driven, 128));
            var chip = PenaltyKickPlanner.ShotSpeed(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Chip, 128));
            Assert.That(driven, Is.GreaterThan(placed));
            Assert.That(placed, Is.GreaterThan(chip));
        }
    }

    public class ShootoutMessagesTests
    {
        [Test]
        public void FitOneBleWriteAndRoundTrip()
        {
            Assert.That(MatchSetup.Size, Is.LessThanOrEqualTo(20));
            Assert.That(LaunchMessage.Size, Is.LessThanOrEqualTo(20));
            Assert.That(ResultMessage.Size, Is.LessThanOrEqualTo(20));

            var setup = new MatchSetup(Era.Seventies, -35, 12, 200, 2240, 5, 0xDEADBEEF);
            var decodedSetup = MatchSetup.Decode(setup.Encode());
            Assert.That(decodedSetup, Is.EqualTo(setup));
            Assert.That(decodedSetup.Conditions.Wind.X, Is.EqualTo(-3.5).Within(1e-12));
            Assert.That(decodedSetup.Conditions.AltitudeMetres, Is.EqualTo(2240));

            var launch = new LaunchParams(Pitch.PenaltySpot, new Vec3(-3.21, 2.5, 27.99), new Vec3(4, -18, 0.01), 5).Quantized();
            var decodedLaunch = LaunchMessage.Decode(new LaunchMessage(9, launch).Encode());
            Assert.That(decodedLaunch.Round, Is.EqualTo(9));
            Assert.That(decodedLaunch.Velocity, Is.EqualTo(launch.Velocity));
            Assert.That(decodedLaunch.Spin, Is.EqualTo(launch.Spin));

            var result = ResultMessage.Decode(new ResultMessage(9, ShotOutcome.Woodwork, new Vec3(3.721, 1.0, 11), 0.512).Encode());
            Assert.That(result.Outcome, Is.EqualTo(ShotOutcome.Woodwork));
            Assert.That(result.EndPoint.X, Is.EqualTo(3.721).Within(1e-12));
            Assert.That(result.FlightSeconds, Is.EqualTo(0.512).Within(1e-12));
        }

        [Test]
        public void RejectsOutOfRangeFields()
        {
            var setup = new MatchSetup(Era.Modern, 0, 0, 0, 0, 5, 1).Encode();
            setup[1] = 7;
            Assert.Throws<ProtocolException>(() => MatchSetup.Decode(setup));

            var result = new ResultMessage(0, ShotOutcome.Goal, Vec3.Zero, 0).Encode();
            result[2] = 9;
            Assert.Throws<ProtocolException>(() => ResultMessage.Decode(result));
        }
    }
}
