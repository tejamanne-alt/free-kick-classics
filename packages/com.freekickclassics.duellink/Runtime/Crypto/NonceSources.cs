using System;
using System.Security.Cryptography;

namespace FreeKickClassics.DuelLink.Crypto
{
    public interface INonceSource
    {
        void Fill(byte[] buffer);
    }

    /// <summary>The operating system's secure random generator. Use this in builds.</summary>
    /// <remarks>
    /// Check on device during the Bluetooth spike that <see cref="RandomNumberGenerator.Create()"/> survives IL2CPP
    /// managed code stripping; if it doesn't, add a link.xml that preserves System.Security.Cryptography.
    /// </remarks>
    public sealed class SystemNonceSource : INonceSource
    {
        readonly RandomNumberGenerator generator = RandomNumberGenerator.Create();

        public void Fill(byte[] buffer) => generator.GetBytes(buffer);
    }

    /// <summary>
    /// Repeatable nonces for tests and replays. Not secure: anyone who knows the seed knows every nonce, so never use
    /// it in a match between two people.
    /// </summary>
    public sealed class InsecureSeededNonceSource : INonceSource
    {
        ulong state;

        public InsecureSeededNonceSource(ulong seed)
        {
            state = seed;
        }

        public void Fill(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            ulong word = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i % 8 == 0) word = Next();
                buffer[i] = (byte)(word >> (8 * (i % 8)));
            }
        }

        ulong Next()
        {
            unchecked
            {
                // SplitMix64.
                ulong z = state += 0x9E3779B97F4A7C15UL;
                z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
                z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
                return z ^ (z >> 31);
            }
        }
    }
}
