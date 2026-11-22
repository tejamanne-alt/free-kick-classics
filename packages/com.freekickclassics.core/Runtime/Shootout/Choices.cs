using System;
using FreeKickClassics.DuelLink.Crypto;

namespace FreeKickClassics.Core.Shootout
{
    /// <summary>Goal columns as the kicker (and the broadcast camera) sees them. The keeper's screen mirrors them.</summary>
    public enum GoalColumn : byte
    {
        Left = 0,
        Centre = 1,
        Right = 2,
    }

    public enum GoalRow : byte
    {
        Low = 0,
        Middle = 1,
        High = 2,
    }

    public enum StrikeType : byte
    {
        /// <summary>Side-foot: accurate, curls away from the keeper, easier to save.</summary>
        Placed = 0,
        /// <summary>Laces: fast and hard to save, less accurate, and it knuckles with a modern ball.</summary>
        Driven = 1,
        /// <summary>Soft chip down the middle or into a corner: humiliates an early dive, gets caught by a keeper who stays big.</summary>
        Chip = 2,
    }

    /// <summary>Dive direction, in the kicker's view like <see cref="GoalColumn"/>.</summary>
    public enum DiveSide : byte
    {
        Left = 0,
        Right = 1,
    }

    public enum DiveTiming : byte
    {
        /// <summary>Go before the ball is struck: covers the corner best, but a chip or a stutter beats it.</summary>
        Early = 0,
        /// <summary>React off the strike: less reach into the corners, but not fooled by a stutter.</summary>
        Late = 1,
        /// <summary>Stay central and make yourself big, leaning to the chosen side. Owns the middle.</summary>
        StayBig = 2,
    }

    /// <summary>The kicker's secret pick for one penalty. 4 bytes on the wire.</summary>
    public readonly struct KickerChoice : IEquatable<KickerChoice>
    {
        public const byte FlagStutter = 0x01;

        public readonly GoalColumn Column;
        public readonly GoalRow Row;
        public readonly StrikeType Strike;
        /// <summary>0-255. More power means a faster ball and more chance of missing the target.</summary>
        public readonly byte Power;
        /// <summary>Stutter in the run-up: wrecks an early dive, costs accuracy.</summary>
        public readonly bool Stutter;

        public KickerChoice(GoalColumn column, GoalRow row, StrikeType strike, byte power, bool stutter = false)
        {
            if ((byte)column > 2) throw new ArgumentOutOfRangeException(nameof(column));
            if ((byte)row > 2) throw new ArgumentOutOfRangeException(nameof(row));
            if ((byte)strike > 2) throw new ArgumentOutOfRangeException(nameof(strike));
            Column = column;
            Row = row;
            Strike = strike;
            Power = power;
            Stutter = stutter;
        }

        /// <summary>0-8, row-major from the bottom-left corner in the kicker's view.</summary>
        public int Zone => (int)Row * 3 + (int)Column;

        public byte[] Encode() => new[] { (byte)Zone, (byte)Strike, Power, Stutter ? FlagStutter : (byte)0 };

        public static bool TryDecode(byte[] payload, out KickerChoice choice)
        {
            choice = default;
            if (payload == null || payload.Length != Commitment.PayloadSize) return false;
            if (payload[0] > 8 || payload[1] > 2 || (payload[3] & ~FlagStutter) != 0) return false;
            choice = new KickerChoice(
                (GoalColumn)(payload[0] % 3),
                (GoalRow)(payload[0] / 3),
                (StrikeType)payload[1],
                payload[2],
                (payload[3] & FlagStutter) != 0);
            return true;
        }

        public bool Equals(KickerChoice other) =>
            Column == other.Column && Row == other.Row && Strike == other.Strike && Power == other.Power && Stutter == other.Stutter;

        public override bool Equals(object obj) => obj is KickerChoice other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Column, Row, Strike, Power, Stutter);

        public override string ToString() => $"{Row}-{Column} {Strike} p{Power}{(Stutter ? " stutter" : "")}";
    }

    /// <summary>The keeper's secret pick for one penalty. 4 bytes on the wire.</summary>
    public readonly struct KeeperChoice : IEquatable<KeeperChoice>
    {
        public const byte FlagShuffle = 0x01;

        public readonly DiveSide Side;
        public readonly DiveTiming Timing;
        /// <summary>Shuffle on the line: shrinks the kicker's sweet spot, costs a little reach.</summary>
        public readonly bool Shuffle;

        public KeeperChoice(DiveSide side, DiveTiming timing, bool shuffle = false)
        {
            if ((byte)side > 1) throw new ArgumentOutOfRangeException(nameof(side));
            if ((byte)timing > 2) throw new ArgumentOutOfRangeException(nameof(timing));
            Side = side;
            Timing = timing;
            Shuffle = shuffle;
        }

        public byte[] Encode() => new[] { (byte)Side, (byte)Timing, (byte)0, Shuffle ? FlagShuffle : (byte)0 };

        public static bool TryDecode(byte[] payload, out KeeperChoice choice)
        {
            choice = default;
            if (payload == null || payload.Length != Commitment.PayloadSize) return false;
            if (payload[0] > 1 || payload[1] > 2 || payload[2] != 0 || (payload[3] & ~FlagShuffle) != 0) return false;
            choice = new KeeperChoice((DiveSide)payload[0], (DiveTiming)payload[1], (payload[3] & FlagShuffle) != 0);
            return true;
        }

        public bool Equals(KeeperChoice other) => Side == other.Side && Timing == other.Timing && Shuffle == other.Shuffle;

        public override bool Equals(object obj) => obj is KeeperChoice other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Side, Timing, Shuffle);

        public override string ToString() => $"{Timing} {Side}{(Shuffle ? " shuffle" : "")}";
    }
}
