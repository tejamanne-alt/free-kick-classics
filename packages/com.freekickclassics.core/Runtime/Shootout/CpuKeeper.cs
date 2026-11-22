using System;
using System.Collections.Generic;

namespace FreeKickClassics.Core.Shootout
{
    public sealed class CpuKeeperProfile
    {
        public static readonly CpuKeeperProfile Easy = new CpuKeeperProfile(recencyHalfLife: 2, priorWeight: 4, temperature: 0.10);
        public static readonly CpuKeeperProfile Normal = new CpuKeeperProfile(recencyHalfLife: 4, priorWeight: 2, temperature: 0.04);
        public static readonly CpuKeeperProfile Hard = new CpuKeeperProfile(recencyHalfLife: 6, priorWeight: 1, temperature: 0.015);

        public CpuKeeperProfile(double recencyHalfLife, double priorWeight, double temperature)
        {
            if (recencyHalfLife <= 0) throw new ArgumentOutOfRangeException(nameof(recencyHalfLife));
            if (priorWeight < 0) throw new ArgumentOutOfRangeException(nameof(priorWeight));
            if (temperature <= 0) throw new ArgumentOutOfRangeException(nameof(temperature));
            RecencyHalfLife = recencyHalfLife;
            PriorWeight = priorWeight;
            Temperature = temperature;
        }

        /// <summary>How many kicks it takes for an old kick to count half as much as the latest one.</summary>
        public double RecencyHalfLife { get; }

        /// <summary>How many kicks' worth of "a typical penalty taker" the keeper assumes before it has seen any.</summary>
        public double PriorWeight { get; }

        /// <summary>
        /// How sharply it picks the best-looking dive, in goal-probability units. Lower is more ruthless, and also
        /// more predictable, which a sharp kicker can exploit.
        /// </summary>
        public double Temperature { get; }
    }

    /// <summary>
    /// The vs-CPU keeper. It reads the kicker's history (one player's, or both players' combined in Tag-Team
    /// Shootout), predicts their next kick, and picks a dive that is good against that prediction, with some
    /// randomness so it can't be farmed.
    /// </summary>
    /// <remarks>
    /// The prediction treats direction, height, strike, power and stutter as independent habits, each a
    /// recency-weighted count blended with a typical kicker. Choices are local to one phone, so it may use floating
    /// point freely.
    /// </remarks>
    public sealed class CpuKeeper
    {
        static readonly double[] PriorColumn = { 0.4, 0.2, 0.4 };
        static readonly double[] PriorRow = { 0.45, 0.35, 0.2 };
        static readonly double[] PriorStrike = { 0.6, 0.3, 0.1 };
        static readonly double[] PriorPower = { 0.25, 0.5, 0.25 };
        static readonly double[] PriorStutter = { 0.85, 0.15 };
        /// <summary>Representative power for each of the three power bands.</summary>
        static readonly byte[] BandPower = { 64, 160, 235 };

        static readonly KeeperChoice[] Options = BuildOptions();

        readonly PenaltyResolver resolver;
        readonly CpuKeeperProfile profile;

        public CpuKeeper(PenaltyResolver resolver, CpuKeeperProfile profile)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public static IReadOnlyList<KeeperChoice> AllOptions => Options;

        public KeeperChoice Choose(IReadOnlyList<KickerChoice> history, DeterministicRandom random)
        {
            double[] expected = ExpectedGoalProbabilities(history);
            double best = double.MaxValue;
            foreach (double e in expected) best = Math.Min(best, e);

            var weights = new double[expected.Length];
            double total = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                weights[i] = Math.Exp(-(expected[i] - best) / profile.Temperature);
                total += weights[i];
            }

            double roll = random.NextDouble() * total;
            for (int i = 0; i < weights.Length; i++)
            {
                roll -= weights[i];
                if (roll < 0) return Options[i];
            }

            return Options[Options.Length - 1];
        }

        /// <summary>Predicted goal chance against each of <see cref="AllOptions"/>, in the same order.</summary>
        public double[] ExpectedGoalProbabilities(IReadOnlyList<KickerChoice> history)
        {
            double[] column = Predict(history, PriorColumn, k => (int)k.Column);
            double[] row = Predict(history, PriorRow, k => (int)k.Row);
            double[] strike = Predict(history, PriorStrike, k => (int)k.Strike);
            double[] power = Predict(history, PriorPower, k => PowerBand(k.Power));
            double[] stutter = Predict(history, PriorStutter, k => k.Stutter ? 1 : 0);

            var expected = new double[Options.Length];
            for (int c = 0; c < 3; c++)
            for (int r = 0; r < 3; r++)
            for (int s = 0; s < 3; s++)
            for (int p = 0; p < 3; p++)
            for (int u = 0; u < 2; u++)
            {
                double likelihood = column[c] * row[r] * strike[s] * power[p] * stutter[u];
                if (likelihood < 1e-6) continue;
                var kick = new KickerChoice((GoalColumn)c, (GoalRow)r, (StrikeType)s, BandPower[p], u == 1);
                for (int o = 0; o < Options.Length; o++)
                {
                    expected[o] += likelihood * resolver.GoalProbability(kick, Options[o]);
                }
            }

            return expected;
        }

        public static int PowerBand(byte power) => power < 112 ? 0 : power < 200 ? 1 : 2;

        double[] Predict(IReadOnlyList<KickerChoice> history, double[] prior, Func<KickerChoice, int> select)
        {
            var counts = new double[prior.Length];
            double total = 0;
            for (int i = 0; i < history.Count; i++)
            {
                double age = history.Count - 1 - i;
                double weight = Math.Pow(0.5, age / profile.RecencyHalfLife);
                counts[select(history[i])] += weight;
                total += weight;
            }

            var probabilities = new double[prior.Length];
            double denominator = total + profile.PriorWeight;
            for (int v = 0; v < prior.Length; v++)
            {
                probabilities[v] = denominator > 0 ? (counts[v] + profile.PriorWeight * prior[v]) / denominator : prior[v];
            }

            return probabilities;
        }

        static KeeperChoice[] BuildOptions()
        {
            var options = new List<KeeperChoice>();
            foreach (DiveTiming timing in new[] { DiveTiming.Early, DiveTiming.Late, DiveTiming.StayBig })
            foreach (DiveSide side in new[] { DiveSide.Left, DiveSide.Right })
            foreach (bool shuffle in new[] { false, true })
            {
                options.Add(new KeeperChoice(side, timing, shuffle));
            }

            return options.ToArray();
        }
    }
}
