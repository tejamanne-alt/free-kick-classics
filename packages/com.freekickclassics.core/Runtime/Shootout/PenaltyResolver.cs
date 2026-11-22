using System;

namespace FreeKickClassics.Core.Shootout
{
    public enum ShotOutcome : byte
    {
        Goal = 0,
        Saved = 1,
        Woodwork = 2,
        Missed = 3,
    }

    /// <summary>What happens to one penalty, decided before any physics runs.</summary>
    public readonly struct ShotResolution : IEquatable<ShotResolution>
    {
        public readonly ShotOutcome Outcome;
        /// <summary>Where the ball's centre crosses the goal-line plane, in millimetres (x right, y up, kicker's view).</summary>
        public readonly int TargetXmm;
        public readonly int TargetYmm;
        public readonly int MissPerMille;
        public readonly int SavePerMille;

        public ShotResolution(ShotOutcome outcome, int targetXmm, int targetYmm, int missPerMille, int savePerMille)
        {
            Outcome = outcome;
            TargetXmm = targetXmm;
            TargetYmm = targetYmm;
            MissPerMille = missPerMille;
            SavePerMille = savePerMille;
        }

        public bool IsGoal => Outcome == ShotOutcome.Goal;

        public bool Equals(ShotResolution other) =>
            Outcome == other.Outcome && TargetXmm == other.TargetXmm && TargetYmm == other.TargetYmm &&
            MissPerMille == other.MissPerMille && SavePerMille == other.SavePerMille;

        public override bool Equals(object obj) => obj is ShotResolution other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Outcome, TargetXmm, TargetYmm, MissPerMille, SavePerMille);

        public override string ToString() => $"{Outcome} at ({TargetXmm}, {TargetYmm}) mm";
    }

    /// <summary>
    /// Settles kicker against keeper with whole-number rules and a shared seed. Both phones run it and must agree.
    /// </summary>
    public sealed class PenaltyResolver
    {
        const int PostAxisXmm = 3720;
        const int BarAxisYmm = 2500;
        /// <summary>Post or bar radius plus ball radius: within this of an axis the ball touches the frame.</summary>
        const int ContactMm = 170;
        const int SafeMarginMm = 20;
        const int BallRadiusMm = 110;

        readonly ShootoutTuning tuning;

        public PenaltyResolver(ShootoutTuning tuning)
        {
            this.tuning = tuning ?? throw new ArgumentNullException(nameof(tuning));
        }

        public ShootoutTuning Tuning => tuning;

        public int MissPerMille(KickerChoice kick, KeeperChoice keeper)
        {
            long miss = tuning.BaseMissPerMille[(int)kick.Strike];
            miss *= tuning.ZoneMissPercent[(int)kick.Row, (int)kick.Column];
            miss *= PowerPercent(kick.Power, tuning.MissPercentAtMinPower, tuning.MissPercentAtMaxPower);
            miss *= kick.Stutter ? tuning.StutterMissPercent : 100;
            miss *= keeper.Shuffle ? tuning.ShuffleMissPercent : 100;
            miss /= 100L * 100 * 100 * 100;
            return (int)System.Math.Min(miss, tuning.MaxMissPerMille);
        }

        /// <summary>Chance the keeper saves the shot if it's on target.</summary>
        public int SavePerMille(KickerChoice kick, KeeperChoice keeper)
        {
            int relativeColumn = RelativeColumn(kick.Column, keeper.Side);
            long save = tuning.CoveragePerMille[(int)keeper.Timing, relativeColumn, (int)kick.Row];
            save *= tuning.StrikeSavePercent[(int)kick.Strike];
            save *= PowerPercent(kick.Power, tuning.SavePercentAtMinPower, tuning.SavePercentAtMaxPower);
            save *= kick.Strike == StrikeType.Chip ? tuning.ChipSavePercentByTiming[(int)keeper.Timing] : 100;
            save *= kick.Stutter && keeper.Timing == DiveTiming.Early ? tuning.StutterVsEarlySavePercent : 100;
            save *= keeper.Shuffle ? tuning.ShuffleSavePercent : 100;
            save /= 100L * 100 * 100 * 100 * 100;
            return (int)System.Math.Min(save, tuning.MaxSavePerMille);
        }

        /// <summary>Exact chance of a goal, for analysis, balance tests and the CPU keeper.</summary>
        public double GoalProbability(KickerChoice kick, KeeperChoice keeper) =>
            (1000 - MissPerMille(kick, keeper)) / 1000.0 * ((1000 - SavePerMille(kick, keeper)) / 1000.0);

        public ShotResolution Resolve(KickerChoice kick, KeeperChoice keeper, ulong seed)
        {
            var random = new DeterministicRandom(seed);
            int miss = MissPerMille(kick, keeper);
            int save = SavePerMille(kick, keeper);

            if (random.NextPerMille() < miss)
            {
                bool woodwork = random.NextPerMille() < tuning.WoodworkSharePerMille;
                OffTargetPoint(kick, woodwork, random, out int missX, out int missY);
                return new ShotResolution(woodwork ? ShotOutcome.Woodwork : ShotOutcome.Missed, missX, missY, miss, save);
            }

            OnTargetPoint(kick, random, out int x, out int y);
            var outcome = random.NextPerMille() < save ? ShotOutcome.Saved : ShotOutcome.Goal;
            return new ShotResolution(outcome, x, y, miss, save);
        }

        /// <summary>0 = the side the keeper went (or leans), 1 = centre, 2 = the far side.</summary>
        public static int RelativeColumn(GoalColumn column, DiveSide side)
        {
            if (column == GoalColumn.Centre) return 1;
            bool sameSide = (column == GoalColumn.Left) == (side == DiveSide.Left);
            return sameSide ? 0 : 2;
        }

        static int PowerPercent(byte power, int atMin, int atMax) => atMin + (atMax - atMin) * power / 255;

        void OnTargetPoint(KickerChoice kick, DeterministicRandom random, out int x, out int y)
        {
            int strike = (int)kick.Strike;
            x = tuning.ColumnCentreXmm[(int)kick.Column] + random.NextInt(-tuning.ScatterXmm[strike], tuning.ScatterXmm[strike]);
            y = tuning.RowCentreYmm[(int)kick.Row] + random.NextInt(-tuning.ScatterYmm[strike], tuning.ScatterYmm[strike]);

            const int maxX = PostAxisXmm - ContactMm - SafeMarginMm;
            const int maxY = BarAxisYmm - ContactMm - SafeMarginMm;
            x = Clamp(x, -maxX, maxX);
            y = Clamp(y, BallRadiusMm + SafeMarginMm, maxY);
        }

        void OffTargetPoint(KickerChoice kick, bool woodwork, DeterministicRandom random, out int x, out int y)
        {
            int columnX = tuning.ColumnCentreXmm[(int)kick.Column];
            int rowY = tuning.RowCentreYmm[(int)kick.Row];
            int sign = kick.Column == GoalColumn.Left ? -1 : 1;

            // The centre column can only go over; outer columns go wide, or either way from the top corners.
            bool overTheBar = kick.Column == GoalColumn.Centre ||
                (kick.Row == GoalRow.High && random.NextPerMille() < 500);

            if (woodwork)
            {
                const int reach = ContactMm - SafeMarginMm;
                if (overTheBar)
                {
                    x = Clamp(columnX + random.NextInt(-400, 400), -(PostAxisXmm - 120), PostAxisXmm - 120);
                    y = BarAxisYmm + random.NextInt(-reach, reach);
                }
                else
                {
                    x = sign * (PostAxisXmm + random.NextInt(-reach, reach));
                    y = Clamp(rowY + random.NextInt(-150, 150), 150, BarAxisYmm - 100);
                }

                return;
            }

            const int clear = ContactMm + 50;
            if (overTheBar)
            {
                x = Clamp(columnX + random.NextInt(-400, 400), -(PostAxisXmm - 120), PostAxisXmm - 120);
                y = BarAxisYmm + clear + random.NextInt(0, 700);
            }
            else
            {
                x = sign * (PostAxisXmm + clear + random.NextInt(0, 900));
                y = Clamp(rowY + random.NextInt(-200, 200), 150, 2300);
            }
        }

        static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;
    }
}
