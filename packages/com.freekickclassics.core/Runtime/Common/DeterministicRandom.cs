namespace FreeKickClassics.Core
{
    /// <summary>
    /// SplitMix64. Integer-only, so the same seed gives the same sequence on every phone and every compiler.
    /// Rules that both phones must agree on draw from this with <see cref="NextInt"/> and <see cref="NextPerMille"/>;
    /// <see cref="NextDouble"/> is for local-only decisions such as the CPU keeper.
    /// </summary>
    public sealed class DeterministicRandom
    {
        ulong state;

        public DeterministicRandom(ulong seed)
        {
            state = seed;
        }

        public ulong NextULong()
        {
            unchecked
            {
                ulong z = state += 0x9E3779B97F4A7C15UL;
                z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
                z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
                return z ^ (z >> 31);
            }
        }

        /// <summary>Uniform in [0, maxExclusive). Bias is below 2^-32, far under anything a player could notice.</summary>
        public int NextInt(int maxExclusive)
        {
            if (maxExclusive <= 0) throw new System.ArgumentOutOfRangeException(nameof(maxExclusive));
            return (int)(((NextULong() >> 32) * (ulong)maxExclusive) >> 32);
        }

        /// <summary>Uniform in [minInclusive, maxInclusive].</summary>
        public int NextInt(int minInclusive, int maxInclusive) => minInclusive + NextInt(maxInclusive - minInclusive + 1);

        public int NextPerMille() => NextInt(1000);

        /// <summary>Uniform in [0, 1).</summary>
        public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

        /// <summary>A stateless hash of (seed, index), for noise that must be sampled at any point in any order.</summary>
        public static ulong Hash(ulong seed, ulong index)
        {
            unchecked
            {
                var random = new DeterministicRandom(seed ^ (index * 0xD1B54A32D192ED03UL));
                return random.NextULong();
            }
        }
    }
}
