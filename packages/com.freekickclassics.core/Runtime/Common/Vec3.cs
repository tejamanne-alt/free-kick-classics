using System;
using System.Globalization;

namespace FreeKickClassics.Core
{
    /// <summary>
    /// Double-precision vector in Unity's axes: X right, Y up, Z forward (from the kicker towards the goal).
    /// Convert to <c>UnityEngine.Vector3</c> only at the presentation layer.
    /// </summary>
    public readonly struct Vec3 : IEquatable<Vec3>
    {
        public static readonly Vec3 Zero = new Vec3(0, 0, 0);
        public static readonly Vec3 Up = new Vec3(0, 1, 0);
        public static readonly Vec3 Right = new Vec3(1, 0, 0);
        public static readonly Vec3 Forward = new Vec3(0, 0, 1);

        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double LengthSquared => X * X + Y * Y + Z * Z;

        public double Length => System.Math.Sqrt(LengthSquared);

        public Vec3 Normalized
        {
            get
            {
                double length = Length;
                return length > 0 ? this / length : Zero;
            }
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vec3 operator -(Vec3 a) => new Vec3(-a.X, -a.Y, -a.Z);

        public static Vec3 operator *(Vec3 a, double s) => new Vec3(a.X * s, a.Y * s, a.Z * s);

        public static Vec3 operator *(double s, Vec3 a) => new Vec3(a.X * s, a.Y * s, a.Z * s);

        public static Vec3 operator /(Vec3 a, double s) => new Vec3(a.X / s, a.Y / s, a.Z / s);

        public static double Dot(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        /// <summary>Standard cross product, the same formula as <c>UnityEngine.Vector3.Cross</c>.</summary>
        public static Vec3 Cross(Vec3 a, Vec3 b) =>
            new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        public static Vec3 Lerp(Vec3 a, Vec3 b, double t) => a + (b - a) * t;

        public bool Equals(Vec3 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override bool Equals(object obj) => obj is Vec3 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator ==(Vec3 a, Vec3 b) => a.Equals(b);

        public static bool operator !=(Vec3 a, Vec3 b) => !a.Equals(b);

        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "({0:F3}, {1:F3}, {2:F3})", X, Y, Z);
    }
}
