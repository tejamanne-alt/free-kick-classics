using System.Collections.Generic;

namespace FreeKickClassics.Core.Shootout
{
    public enum TendencyKind : byte
    {
        Column,
        Row,
        Strike,
        Stutter,
        DiveSide,
        DiveTiming,
        Shuffle,
    }

    /// <summary>
    /// A habit worth showing after a kick, like "went left 4 of 5". <see cref="Value"/> is the enum value for its
    /// <see cref="Kind"/> (a <see cref="GoalColumn"/>, <see cref="StrikeType"/> and so on), or 1 for the yes/no habits.
    /// Wording and localisation belong to the UI.
    /// </summary>
    public readonly struct Tendency
    {
        public readonly TendencyKind Kind;
        public readonly int Value;
        public readonly int Count;
        public readonly int OutOf;

        public Tendency(TendencyKind kind, int value, int count, int outOf)
        {
            Kind = kind;
            Value = value;
            Count = count;
            OutOf = outOf;
        }

        public double Share => OutOf == 0 ? 0 : (double)Count / OutOf;

        public override string ToString() => $"{Kind}={Value} {Count} of {OutOf}";
    }

    public static class TendencyReader
    {
        public const int DefaultWindow = 5;
        public const int MinimumSamples = 3;
        /// <summary>A habit is only shown once it covers at least this share of recent picks.</summary>
        public const double MinimumShare = 0.6;

        /// <summary>The kicker's most pronounced habit over their last <paramref name="window"/> kicks.</summary>
        public static bool TryGetStrongest(IReadOnlyList<KickerChoice> history, out Tendency tendency, int window = DefaultWindow)
        {
            var candidates = new List<Tendency>();
            int start = Start(history.Count, window);
            int outOf = history.Count - start;
            if (outOf >= MinimumSamples)
            {
                candidates.Add(Most(history, start, TendencyKind.Column, 3, k => (int)k.Column));
                candidates.Add(Most(history, start, TendencyKind.Row, 3, k => (int)k.Row));
                candidates.Add(Most(history, start, TendencyKind.Strike, 3, k => (int)k.Strike));
                candidates.Add(Yes(history, start, TendencyKind.Stutter, k => k.Stutter));
            }

            return Pick(candidates, out tendency);
        }

        /// <summary>The keeper's most pronounced habit over their last <paramref name="window"/> saves.</summary>
        public static bool TryGetStrongest(IReadOnlyList<KeeperChoice> history, out Tendency tendency, int window = DefaultWindow)
        {
            var candidates = new List<Tendency>();
            int start = Start(history.Count, window);
            int outOf = history.Count - start;
            if (outOf >= MinimumSamples)
            {
                candidates.Add(Most(history, start, TendencyKind.DiveSide, 2, k => (int)k.Side));
                candidates.Add(Most(history, start, TendencyKind.DiveTiming, 3, k => (int)k.Timing));
                candidates.Add(Yes(history, start, TendencyKind.Shuffle, k => k.Shuffle));
            }

            return Pick(candidates, out tendency);
        }

        static int Start(int count, int window) => window <= 0 || window >= count ? 0 : count - window;

        static Tendency Most<T>(IReadOnlyList<T> history, int start, TendencyKind kind, int values, System.Func<T, int> select)
        {
            var counts = new int[values];
            for (int i = start; i < history.Count; i++) counts[select(history[i])]++;
            int best = 0;
            for (int v = 1; v < values; v++)
            {
                if (counts[v] > counts[best]) best = v;
            }

            return new Tendency(kind, best, counts[best], history.Count - start);
        }

        static Tendency Yes<T>(IReadOnlyList<T> history, int start, TendencyKind kind, System.Func<T, bool> select)
        {
            int count = 0;
            for (int i = start; i < history.Count; i++)
            {
                if (select(history[i])) count++;
            }

            return new Tendency(kind, 1, count, history.Count - start);
        }

        /// <summary>Highest share wins; ties go to the earlier kind, so direction beats height beats strike.</summary>
        static bool Pick(List<Tendency> candidates, out Tendency tendency)
        {
            tendency = default;
            bool found = false;
            foreach (var candidate in candidates)
            {
                if (candidate.Share < MinimumShare) continue;
                if (!found || candidate.Count * tendency.OutOf > tendency.Count * candidate.OutOf)
                {
                    tendency = candidate;
                    found = true;
                }
            }

            return found;
        }
    }
}
