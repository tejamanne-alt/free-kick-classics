using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using FreeKickClassics.DuelLink.Crypto;

namespace FreeKickClassics.Core.Shootout
{
    /// <summary>
    /// The numbers behind Stare-Down Shootout. All whole numbers (per-mille chances and percent multipliers), so
    /// both phones resolve a penalty identically and the guest can check the host's result.
    /// </summary>
    /// <remarks>
    /// Both phones must run the same tuning. The host sends <see cref="Fingerprint"/> in the match setup and the guest
    /// refuses to play on a mismatch, so a Remote Config change can't split a match between two rule sets.
    /// <c>PenaltyBalanceTests</c> keeps the defaults honest: every keeper timing has a job, and the equilibrium goal
    /// rate stays close to real football's.
    /// </remarks>
    public sealed class ShootoutTuning
    {
        // ---- Accuracy: chance the shot misses the target, before it ever reaches the keeper. ----

        /// <summary>Per-mille base miss chance by <see cref="StrikeType"/>.</summary>
        public int[] BaseMissPerMille = { 40, 90, 80 };

        /// <summary>Percent multiplier by [row, column]: corners are risky, the low and middle centre never miss.</summary>
        public int[,] ZoneMissPercent =
        {
            { 100, 0, 100 },
            { 110, 0, 110 },
            { 180, 70, 180 },
        };

        public int MissPercentAtMinPower = 60;
        public int MissPercentAtMaxPower = 190;
        public int StutterMissPercent = 160;
        public int ShuffleMissPercent = 135;
        public int MaxMissPerMille = 600;

        /// <summary>Per-mille share of misses that hit the post or bar instead of going wide or over.</summary>
        public int WoodworkSharePerMille = 220;

        // ---- Saves: chance the keeper stops an on-target shot. ----

        /// <summary>
        /// Per-mille save chance by [timing, column relative to the dive, row]. Relative column 0 is the side the keeper
        /// chose (the lean side for <see cref="DiveTiming.StayBig"/>), 1 the centre, 2 the far side.
        /// </summary>
        public int[,,] CoveragePerMille =
        {
            // Early
            { { 880, 820, 560 }, { 120, 70, 40 }, { 0, 0, 0 } },
            // Late
            { { 650, 600, 380 }, { 380, 320, 160 }, { 70, 50, 10 } },
            // StayBig
            { { 260, 320, 120 }, { 820, 880, 640 }, { 160, 220, 60 } },
        };

        /// <summary>Percent save multiplier by <see cref="StrikeType"/>: driven shots are harder to stop, chips easier.</summary>
        public int[] StrikeSavePercent = { 100, 80, 110 };

        public int SavePercentAtMinPower = 115;
        public int SavePercentAtMaxPower = 85;

        /// <summary>Percent save multiplier for a chip by <see cref="DiveTiming"/>: an early diver is already on the floor.</summary>
        public int[] ChipSavePercentByTiming = { 10, 90, 125 };

        /// <summary>An early diver who meets a stutter has gone too soon.</summary>
        public int StutterVsEarlySavePercent = 35;

        public int ShuffleSavePercent = 90;
        public int MaxSavePerMille = 950;

        // ---- Where the ball goes, in millimetres on the goal-line plane. ----

        public int[] ColumnCentreXmm = { -2600, 0, 2600 };
        public int[] RowCentreYmm = { 350, 1200, 2000 };

        /// <summary>Half-size of the on-target scatter box by <see cref="StrikeType"/>.</summary>
        public int[] ScatterXmm = { 450, 650, 400 };
        public int[] ScatterYmm = { 220, 300, 250 };

        public static ShootoutTuning CreateDefault() => new ShootoutTuning();

        /// <summary>A 32-bit hash of every number above. Equal fingerprints mean equal rules.</summary>
        public uint Fingerprint()
        {
            var values = new List<int>();
            values.AddRange(BaseMissPerMille);
            AddAll(values, ZoneMissPercent);
            values.Add(MissPercentAtMinPower);
            values.Add(MissPercentAtMaxPower);
            values.Add(StutterMissPercent);
            values.Add(ShuffleMissPercent);
            values.Add(MaxMissPerMille);
            values.Add(WoodworkSharePerMille);
            foreach (int v in CoveragePerMille) values.Add(v);
            values.Add(CoveragePerMille.GetLength(0));
            values.Add(CoveragePerMille.GetLength(1));
            values.Add(CoveragePerMille.GetLength(2));
            values.AddRange(StrikeSavePercent);
            values.Add(SavePercentAtMinPower);
            values.Add(SavePercentAtMaxPower);
            values.AddRange(ChipSavePercentByTiming);
            values.Add(StutterVsEarlySavePercent);
            values.Add(ShuffleSavePercent);
            values.Add(MaxSavePerMille);
            values.AddRange(ColumnCentreXmm);
            values.AddRange(RowCentreYmm);
            values.AddRange(ScatterXmm);
            values.AddRange(ScatterYmm);

            var bytes = new byte[values.Count * 4];
            for (int i = 0; i < values.Count; i++) BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(i * 4), values[i]);
            return BinaryPrimitives.ReadUInt32LittleEndian(Sha256.Hash(bytes));
        }

        static void AddAll(List<int> values, int[,] table)
        {
            values.Add(table.GetLength(0));
            values.Add(table.GetLength(1));
            foreach (int v in table) values.Add(v);
        }
    }
}
