using System;

namespace FreeKickClassics.Core.Physics
{
    /// <summary>Everything needed to replay a kick's flight: where it starts, how it leaves the boot, and its wobble seed.</summary>
    public readonly struct LaunchParams
    {
        public readonly Vec3 Position;
        /// <summary>m/s.</summary>
        public readonly Vec3 Velocity;
        /// <summary>
        /// Angular velocity in rad/s, with surface velocity = Cross(Spin, r) as in Unity's Rigidbody. +Y spin curls the
        /// ball right (+X), +X spin is topspin (dips), -X is backspin.
        /// </summary>
        public readonly Vec3 Spin;
        public readonly ulong KnuckleSeed;

        public LaunchParams(Vec3 position, Vec3 velocity, Vec3 spin, ulong knuckleSeed)
        {
            Position = position;
            Velocity = velocity;
            Spin = spin;
            KnuckleSeed = knuckleSeed;
        }

        /// <summary>
        /// Rounds velocity and spin to exactly what the wire carries. The host simulates the quantized launch, so both
        /// phones start from bit-identical numbers.
        /// </summary>
        public LaunchParams Quantized() =>
            new LaunchParams(Position, WireUnits.Quantize(Velocity), WireUnits.Quantize(Spin), KnuckleSeed);
    }

    /// <summary>Fixed-point units for sending physics values in 16 bits.</summary>
    public static class WireUnits
    {
        /// <summary>Hundredths: ±327.67 m/s or rad/s, steps of 0.01.</summary>
        public static short ToCenti(double value) => ToInt16(value * 100.0, value);

        public static double FromCenti(short value) => value / 100.0;

        /// <summary>Millimetres: ±32.767 m.</summary>
        public static short ToMillimetres(double metres) => ToInt16(metres * 1000.0, metres);

        public static double FromMillimetres(short value) => value / 1000.0;

        public static Vec3 Quantize(Vec3 v) =>
            new Vec3(FromCenti(ToCenti(v.X)), FromCenti(ToCenti(v.Y)), FromCenti(ToCenti(v.Z)));

        static short ToInt16(double scaled, double original)
        {
            double rounded = System.Math.Round(scaled, MidpointRounding.AwayFromZero);
            if (double.IsNaN(rounded) || rounded < short.MinValue || rounded > short.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(original), original, "Value does not fit the 16-bit wire format.");
            }

            return (short)rounded;
        }
    }
}
