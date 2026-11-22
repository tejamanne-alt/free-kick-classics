using System;
using System.Buffers.Binary;

namespace FreeKickClassics.DuelLink.Crypto
{
    /// <summary>
    /// Values both phones derive from material they've exchanged, so neither phone can pick them alone.
    /// </summary>
    public static class SharedRandomness
    {
        public const int SessionIdSize = 8;

        /// <summary>Session id from the two Hello nonces. Host nonce first.</summary>
        public static byte[] SessionId(ReadOnlySpan<byte> hostHelloNonce, ReadOnlySpan<byte> guestHelloNonce)
        {
            var hash = HashTagged("DLS1", hostHelloNonce, guestHelloNonce, ReadOnlySpan<byte>.Empty, 0);
            return hash.AsSpan(0, SessionIdSize).ToArray();
        }

        /// <summary>
        /// A coin toss both phones agree on, e.g. who kicks first. Fair as long as either Hello nonce was random.
        /// </summary>
        public static bool CoinToss(ReadOnlySpan<byte> sessionId)
        {
            var hash = HashTagged("DLT1", sessionId, ReadOnlySpan<byte>.Empty, ReadOnlySpan<byte>.Empty, 0);
            return (hash[0] & 1) == 1;
        }

        /// <summary>
        /// Seed for a round's dice rolls (accuracy, scatter, knuckle wobble), from both phones' revealed nonces.
        /// Neither phone knows it until both have committed, so neither can bias it, and both compute the same value,
        /// so the guest can check the host's result.
        /// </summary>
        public static ulong RoundSeed(ReadOnlySpan<byte> sessionId, int round, ReadOnlySpan<byte> hostNonce, ReadOnlySpan<byte> guestNonce)
        {
            var hash = HashTagged("DLR1", sessionId, hostNonce, guestNonce, round);
            return BinaryPrimitives.ReadUInt64LittleEndian(hash);
        }

        static byte[] HashTagged(string tag, ReadOnlySpan<byte> a, ReadOnlySpan<byte> b, ReadOnlySpan<byte> c, int number)
        {
            var input = new byte[4 + 1 + a.Length + 1 + b.Length + 1 + c.Length + 4];
            int offset = 0;
            for (int i = 0; i < 4; i++) input[offset++] = (byte)tag[i];
            offset = AppendWithLength(input, offset, a);
            offset = AppendWithLength(input, offset, b);
            offset = AppendWithLength(input, offset, c);
            BinaryPrimitives.WriteInt32LittleEndian(input.AsSpan(offset), number);
            return Sha256.Hash(input);
        }

        static int AppendWithLength(byte[] buffer, int offset, ReadOnlySpan<byte> part)
        {
            if (part.Length > byte.MaxValue) throw new ArgumentException("Part too long.");
            buffer[offset++] = (byte)part.Length;
            part.CopyTo(buffer.AsSpan(offset));
            return offset + part.Length;
        }
    }
}
