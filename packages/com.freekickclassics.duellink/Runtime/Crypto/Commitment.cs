using System;
using System.Buffers.Binary;

namespace FreeKickClassics.DuelLink.Crypto
{
    /// <summary>
    /// Commit-reveal for secret picks. Each phone first sends <see cref="Compute"/> of its pick, and only reveals the
    /// pick and nonce once it holds the other phone's commitment. Neither phone can change its pick after seeing the
    /// other's, and a hacked phone can't read a pick out of the commitment.
    /// </summary>
    /// <remarks>
    /// The hash covers the session id, the round and which phone committed, so a commitment can't be replayed from an
    /// earlier match or round, and a guest can't copy the host's commitment and later "reveal" the host's own pick
    /// (which would let a keeper mirror the kicker). The nonce is 96 random bits: the pick space is tiny, so without
    /// it the other phone could simply hash every possible pick.
    /// </remarks>
    public static class Commitment
    {
        /// <summary>SHA-256 truncated to 128 bits, so a Commit message fits one 20-byte BLE write.</summary>
        public const int Size = 16;
        public const int NonceSize = 12;
        /// <summary>A pick is always encoded in 4 bytes; what they mean is up to the game.</summary>
        public const int PayloadSize = 4;
        public const int SessionIdSize = SharedRandomness.SessionIdSize;

        static readonly byte[] Domain = { (byte)'D', (byte)'L', (byte)'C', (byte)'1' };

        public static byte[] Compute(ReadOnlySpan<byte> sessionId, int round, DuelRole committer, ReadOnlySpan<byte> payload, ReadOnlySpan<byte> nonce)
        {
            if (sessionId.Length != SessionIdSize) throw new ArgumentException("Session id must be " + SessionIdSize + " bytes.", nameof(sessionId));
            if (payload.Length != PayloadSize) throw new ArgumentException("Payload must be " + PayloadSize + " bytes.", nameof(payload));
            if (nonce.Length != NonceSize) throw new ArgumentException("Nonce must be " + NonceSize + " bytes.", nameof(nonce));

            Span<byte> input = stackalloc byte[4 + SessionIdSize + 4 + 1 + PayloadSize + NonceSize];
            int offset = 0;
            Domain.CopyTo(input);
            offset += 4;
            sessionId.CopyTo(input.Slice(offset));
            offset += SessionIdSize;
            BinaryPrimitives.WriteInt32LittleEndian(input.Slice(offset), round);
            offset += 4;
            input[offset++] = (byte)committer;
            payload.CopyTo(input.Slice(offset));
            offset += PayloadSize;
            nonce.CopyTo(input.Slice(offset));

            Span<byte> hash = stackalloc byte[Sha256.HashSize];
            Sha256.Hash(input, hash);
            return hash.Slice(0, Size).ToArray();
        }

        public static bool Verify(ReadOnlySpan<byte> commitment, ReadOnlySpan<byte> sessionId, int round, DuelRole committer, ReadOnlySpan<byte> payload, ReadOnlySpan<byte> nonce)
        {
            if (commitment.Length != Size) return false;
            var expected = Compute(sessionId, round, committer, payload, nonce);
            int difference = 0;
            for (int i = 0; i < Size; i++) difference |= expected[i] ^ commitment[i];
            return difference == 0;
        }
    }
}
