using System;
using System.Buffers.Binary;
using FreeKickClassics.DuelLink.Crypto;

namespace FreeKickClassics.DuelLink.Wire
{
    /// <summary>
    /// First byte of every message. 0x01-0x3F belong to the link layer (this package); each game defines its own
    /// types from <see cref="FirstGameType"/> up.
    /// </summary>
    public static class LinkMessageType
    {
        public const byte Hello = 0x01;
        public const byte Commit = 0x02;
        public const byte Reveal = 0x03;
        public const byte Ping = 0x04;
        public const byte Pong = 0x05;
        public const byte Abort = 0x06;

        public const byte FirstGameType = 0x40;
    }

    /// <summary>Opening message from each phone. 12 bytes.</summary>
    public readonly struct HelloMessage
    {
        public const int Size = 12;
        public const int NonceSize = 8;

        public readonly byte ProtocolVersion;
        /// <summary>Which game this phone is running, so a Dunk Duel phone can't join a Free Kick room.</summary>
        public readonly byte GameId;
        public readonly DuelRole Role;
        /// <summary>Fresh random bytes; both nonces together derive the session id.</summary>
        public readonly byte[] Nonce;

        public HelloMessage(byte protocolVersion, byte gameId, DuelRole role, byte[] nonce)
        {
            if (nonce == null || nonce.Length != NonceSize) throw new ArgumentException("Nonce must be " + NonceSize + " bytes.", nameof(nonce));
            ProtocolVersion = protocolVersion;
            GameId = gameId;
            Role = role;
            Nonce = nonce;
        }

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = LinkMessageType.Hello;
            data[1] = ProtocolVersion;
            data[2] = GameId;
            data[3] = (byte)Role;
            Nonce.CopyTo(data, 4);
            return data;
        }

        public static HelloMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, LinkMessageType.Hello, Size, "Hello");
            if (data[3] > (byte)DuelRole.Guest) throw new ProtocolException("Hello: unknown role " + data[3] + ".");
            return new HelloMessage(data[1], data[2], (DuelRole)data[3], WireChecks.Slice(data, 4, NonceSize));
        }
    }

    /// <summary>"I have locked in my pick, and this is its hash." 18 bytes.</summary>
    public readonly struct CommitMessage
    {
        public const int Size = 2 + Crypto.Commitment.Size;

        /// <summary>Round number modulo 256.</summary>
        public readonly byte Round;
        public readonly byte[] Commitment;

        public CommitMessage(byte round, byte[] commitment)
        {
            if (commitment == null || commitment.Length != Crypto.Commitment.Size)
            {
                throw new ArgumentException("Commitment must be " + Crypto.Commitment.Size + " bytes.", nameof(commitment));
            }

            Round = round;
            Commitment = commitment;
        }

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = LinkMessageType.Commit;
            data[1] = Round;
            Commitment.CopyTo(data, 2);
            return data;
        }

        public static CommitMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, LinkMessageType.Commit, Size, "Commit");
            return new CommitMessage(data[1], WireChecks.Slice(data, 2, Crypto.Commitment.Size));
        }
    }

    /// <summary>"Here is the pick I committed to, and the nonce that proves it." 18 bytes.</summary>
    public readonly struct RevealMessage
    {
        public const int Size = 2 + Commitment.PayloadSize + Commitment.NonceSize;

        public readonly byte Round;
        public readonly byte[] Payload;
        public readonly byte[] Nonce;

        public RevealMessage(byte round, byte[] payload, byte[] nonce)
        {
            if (payload == null || payload.Length != Commitment.PayloadSize)
            {
                throw new ArgumentException("Payload must be " + Commitment.PayloadSize + " bytes.", nameof(payload));
            }

            if (nonce == null || nonce.Length != Commitment.NonceSize)
            {
                throw new ArgumentException("Nonce must be " + Commitment.NonceSize + " bytes.", nameof(nonce));
            }

            Round = round;
            Payload = payload;
            Nonce = nonce;
        }

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = LinkMessageType.Reveal;
            data[1] = Round;
            Payload.CopyTo(data, 2);
            Nonce.CopyTo(data, 2 + Commitment.PayloadSize);
            return data;
        }

        public static RevealMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, LinkMessageType.Reveal, Size, "Reveal");
            return new RevealMessage(
                data[1],
                WireChecks.Slice(data, 2, Commitment.PayloadSize),
                WireChecks.Slice(data, 2 + Commitment.PayloadSize, Commitment.NonceSize));
        }
    }

    /// <summary>Ping or Pong for measuring round-trip time. 3 bytes.</summary>
    public readonly struct PingMessage
    {
        public const int Size = 3;

        public readonly bool IsPong;
        public readonly ushort Sequence;

        public PingMessage(bool isPong, ushort sequence)
        {
            IsPong = isPong;
            Sequence = sequence;
        }

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = IsPong ? LinkMessageType.Pong : LinkMessageType.Ping;
            BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(1), Sequence);
            return data;
        }

        public static PingMessage Decode(byte[] data)
        {
            if (data == null || data.Length != Size || (data[0] != LinkMessageType.Ping && data[0] != LinkMessageType.Pong))
            {
                throw new ProtocolException("Ping/Pong: expected " + Size + " bytes.");
            }

            return new PingMessage(data[0] == LinkMessageType.Pong, BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(1)));
        }
    }

    /// <summary>"I'm ending the match, and why." 2 bytes. The reason codes belong to the game.</summary>
    public readonly struct AbortMessage
    {
        public const int Size = 2;

        public readonly byte Reason;

        public AbortMessage(byte reason)
        {
            Reason = reason;
        }

        public byte[] Encode() => new[] { LinkMessageType.Abort, Reason };

        public static AbortMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, LinkMessageType.Abort, Size, "Abort");
            return new AbortMessage(data[1]);
        }
    }

    public static class WireChecks
    {
        public static void Expect(byte[] data, byte type, int size, string name)
        {
            if (data == null || data.Length == 0) throw new ProtocolException(name + ": empty message.");
            if (data[0] != type) throw new ProtocolException(name + ": wrong type byte 0x" + data[0].ToString("X2") + ".");
            if (data.Length != size) throw new ProtocolException(name + ": expected " + size + " bytes, got " + data.Length + ".");
        }

        public static byte[] Slice(byte[] data, int offset, int length)
        {
            var slice = new byte[length];
            Buffer.BlockCopy(data, offset, slice, 0, length);
            return slice;
        }

        /// <summary>
        /// Places a received 8-bit round number relative to the local round: 0 for the current round, negative for
        /// an older round (a replayed duplicate), positive for a round that hasn't started here yet.
        /// </summary>
        public static int RoundDelta(byte received, int localRound) => unchecked((sbyte)(byte)(received - localRound));
    }
}
