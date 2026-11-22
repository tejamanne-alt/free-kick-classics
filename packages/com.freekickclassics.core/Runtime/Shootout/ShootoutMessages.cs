using System;
using System.Buffers.Binary;
using FreeKickClassics.Core.Physics;
using FreeKickClassics.DuelLink;
using FreeKickClassics.DuelLink.Wire;

namespace FreeKickClassics.Core.Shootout
{
    public static class ShootoutMessageType
    {
        public const byte MatchSetup = LinkMessageType.FirstGameType;
        public const byte Launch = LinkMessageType.FirstGameType + 1;
        public const byte Result = LinkMessageType.FirstGameType + 2;
    }

    /// <summary>
    /// What the host decides before the first kick: era, weather, stadium altitude, shootout length and the rule
    /// set's fingerprint. 12 bytes. Both phones build their conditions from the decoded values, so they match exactly.
    /// </summary>
    public sealed class MatchSetup : IEquatable<MatchSetup>
    {
        public const int Size = 12;

        public MatchSetup(Era era, sbyte windXDecimetresPerSecond, sbyte windZDecimetresPerSecond, byte rain, ushort altitudeMetres, byte kicksPerSide, uint tuningFingerprint)
        {
            if ((byte)era > (byte)Era.Modern) throw new ArgumentOutOfRangeException(nameof(era));
            if (kicksPerSide < 1) throw new ArgumentOutOfRangeException(nameof(kicksPerSide));
            if (altitudeMetres > 6000) throw new ArgumentOutOfRangeException(nameof(altitudeMetres));
            Era = era;
            WindXDecimetresPerSecond = windXDecimetresPerSecond;
            WindZDecimetresPerSecond = windZDecimetresPerSecond;
            Rain = rain;
            AltitudeMetres = altitudeMetres;
            KicksPerSide = kicksPerSide;
            TuningFingerprint = tuningFingerprint;
        }

        public Era Era { get; }

        /// <summary>Crosswind in tenths of a metre per second; positive blows to the kicker's right.</summary>
        public sbyte WindXDecimetresPerSecond { get; }

        /// <summary>Tail (positive) or head (negative) wind in tenths of a metre per second.</summary>
        public sbyte WindZDecimetresPerSecond { get; }

        /// <summary>0 dry to 255 monsoon.</summary>
        public byte Rain { get; }

        public ushort AltitudeMetres { get; }

        public byte KicksPerSide { get; }

        public uint TuningFingerprint { get; }

        public MatchConditions Conditions =>
            new MatchConditions(new Vec3(WindXDecimetresPerSecond / 10.0, 0, WindZDecimetresPerSecond / 10.0), Rain / 255.0, AltitudeMetres);

        public static MatchSetup Standard(ShootoutTuning tuning, Era era = Era.Modern) =>
            new MatchSetup(era, 0, 0, 0, 0, 5, tuning.Fingerprint());

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = ShootoutMessageType.MatchSetup;
            data[1] = (byte)Era;
            data[2] = unchecked((byte)WindXDecimetresPerSecond);
            data[3] = unchecked((byte)WindZDecimetresPerSecond);
            data[4] = Rain;
            BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(5), AltitudeMetres);
            data[7] = KicksPerSide;
            BinaryPrimitives.WriteUInt32LittleEndian(data.AsSpan(8), TuningFingerprint);
            return data;
        }

        public static MatchSetup Decode(byte[] data)
        {
            WireChecks.Expect(data, ShootoutMessageType.MatchSetup, Size, "MatchSetup");
            ushort altitude = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(5));
            if (data[1] > (byte)Era.Modern || data[7] < 1 || altitude > 6000) throw new ProtocolException("MatchSetup: field out of range.");
            return new MatchSetup(
                (Era)data[1],
                unchecked((sbyte)data[2]),
                unchecked((sbyte)data[3]),
                data[4],
                altitude,
                data[7],
                BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(8)));
        }

        public bool Equals(MatchSetup other) =>
            other != null && Era == other.Era && WindXDecimetresPerSecond == other.WindXDecimetresPerSecond &&
            WindZDecimetresPerSecond == other.WindZDecimetresPerSecond && Rain == other.Rain &&
            AltitudeMetres == other.AltitudeMetres && KicksPerSide == other.KicksPerSide &&
            TuningFingerprint == other.TuningFingerprint;

        public override bool Equals(object obj) => Equals(obj as MatchSetup);

        public override int GetHashCode() => HashCode.Combine(Era, WindXDecimetresPerSecond, WindZDecimetresPerSecond, Rain, AltitudeMetres, KicksPerSide, TuningFingerprint);
    }

    /// <summary>Host to guest: how the ball leaves the boot. 14 bytes. The start point is the penalty spot.</summary>
    public readonly struct LaunchMessage
    {
        public const int Size = 14;

        public readonly byte Round;
        public readonly short VelocityX, VelocityY, VelocityZ;
        public readonly short SpinX, SpinY, SpinZ;

        public LaunchMessage(byte round, LaunchParams launch)
        {
            Round = round;
            VelocityX = WireUnits.ToCenti(launch.Velocity.X);
            VelocityY = WireUnits.ToCenti(launch.Velocity.Y);
            VelocityZ = WireUnits.ToCenti(launch.Velocity.Z);
            SpinX = WireUnits.ToCenti(launch.Spin.X);
            SpinY = WireUnits.ToCenti(launch.Spin.Y);
            SpinZ = WireUnits.ToCenti(launch.Spin.Z);
        }

        LaunchMessage(byte round, short vx, short vy, short vz, short sx, short sy, short sz)
        {
            Round = round;
            VelocityX = vx;
            VelocityY = vy;
            VelocityZ = vz;
            SpinX = sx;
            SpinY = sy;
            SpinZ = sz;
        }

        public Vec3 Velocity => new Vec3(WireUnits.FromCenti(VelocityX), WireUnits.FromCenti(VelocityY), WireUnits.FromCenti(VelocityZ));

        public Vec3 Spin => new Vec3(WireUnits.FromCenti(SpinX), WireUnits.FromCenti(SpinY), WireUnits.FromCenti(SpinZ));

        public LaunchParams ToLaunch(Vec3 start, ulong knuckleSeed) => new LaunchParams(start, Velocity, Spin, knuckleSeed);

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = ShootoutMessageType.Launch;
            data[1] = Round;
            var span = data.AsSpan(2);
            BinaryPrimitives.WriteInt16LittleEndian(span, VelocityX);
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(2), VelocityY);
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(4), VelocityZ);
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(6), SpinX);
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(8), SpinY);
            BinaryPrimitives.WriteInt16LittleEndian(span.Slice(10), SpinZ);
            return data;
        }

        public static LaunchMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, ShootoutMessageType.Launch, Size, "Launch");
            var span = data.AsSpan(2);
            return new LaunchMessage(
                data[1],
                BinaryPrimitives.ReadInt16LittleEndian(span),
                BinaryPrimitives.ReadInt16LittleEndian(span.Slice(2)),
                BinaryPrimitives.ReadInt16LittleEndian(span.Slice(4)),
                BinaryPrimitives.ReadInt16LittleEndian(span.Slice(6)),
                BinaryPrimitives.ReadInt16LittleEndian(span.Slice(8)),
                BinaryPrimitives.ReadInt16LittleEndian(span.Slice(10)));
        }
    }

    /// <summary>Host to guest: the outcome and where the host's flight ended. 11 bytes.</summary>
    public readonly struct ResultMessage
    {
        public const int Size = 11;

        public readonly byte Round;
        public readonly ShotOutcome Outcome;
        public readonly short EndXmm, EndYmm, EndZmm;
        public readonly ushort FlightMilliseconds;

        public ResultMessage(byte round, ShotOutcome outcome, Vec3 endPoint, double flightSeconds)
        {
            Round = round;
            Outcome = outcome;
            EndXmm = WireUnits.ToMillimetres(endPoint.X);
            EndYmm = WireUnits.ToMillimetres(endPoint.Y);
            EndZmm = WireUnits.ToMillimetres(endPoint.Z);
            FlightMilliseconds = (ushort)System.Math.Max(0, System.Math.Min(ushort.MaxValue, System.Math.Round(flightSeconds * 1000)));
        }

        ResultMessage(byte round, ShotOutcome outcome, short x, short y, short z, ushort milliseconds)
        {
            Round = round;
            Outcome = outcome;
            EndXmm = x;
            EndYmm = y;
            EndZmm = z;
            FlightMilliseconds = milliseconds;
        }

        public Vec3 EndPoint => new Vec3(WireUnits.FromMillimetres(EndXmm), WireUnits.FromMillimetres(EndYmm), WireUnits.FromMillimetres(EndZmm));

        public double FlightSeconds => FlightMilliseconds / 1000.0;

        public byte[] Encode()
        {
            var data = new byte[Size];
            data[0] = ShootoutMessageType.Result;
            data[1] = Round;
            data[2] = (byte)Outcome;
            BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan(3), EndXmm);
            BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan(5), EndYmm);
            BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan(7), EndZmm);
            BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(9), FlightMilliseconds);
            return data;
        }

        public static ResultMessage Decode(byte[] data)
        {
            WireChecks.Expect(data, ShootoutMessageType.Result, Size, "Result");
            if (data[2] > (byte)ShotOutcome.Missed) throw new ProtocolException("Result: unknown outcome " + data[2] + ".");
            return new ResultMessage(
                data[1],
                (ShotOutcome)data[2],
                BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(3)),
                BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(5)),
                BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(7)),
                BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(9)));
        }
    }
}
