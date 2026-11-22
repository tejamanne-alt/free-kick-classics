using System;

namespace FreeKickClassics.Core.Physics
{
    public enum Era : byte
    {
        Seventies = 0,
        Nineties = 1,
        Modern = 2,
    }

    /// <summary>
    /// How the ball of an era flies. The same strike plays three different ways: the leather ball barely swerves and
    /// soaks up rain, the glossy ball bends the most, and the light modern ball knuckles.
    /// </summary>
    /// <remarks>
    /// Values are starting points for tuning by feel, in the range of published wind-tunnel figures for size-5 balls,
    /// not measurements of any real ball.
    /// </remarks>
    public sealed class EraProfile
    {
        public static readonly EraProfile Seventies = new EraProfile(
            Era.Seventies,
            ballMassKg: 0.45,
            dragCoefficient: 0.30,
            liftSlope: 0.55,
            liftMax: 0.20,
            knuckleCoefficient: 0.015,
            knuckleSpinThreshold: 10,
            knuckleSegmentSeconds: 0.22,
            waterAbsorption: 0.20,
            spinDecayPerSecond: 0.10,
            restitution: 0.55);

        public static readonly EraProfile Nineties = new EraProfile(
            Era.Nineties,
            ballMassKg: 0.43,
            dragCoefficient: 0.25,
            liftSlope: 1.15,
            liftMax: 0.38,
            knuckleCoefficient: 0.03,
            knuckleSpinThreshold: 10,
            knuckleSegmentSeconds: 0.18,
            waterAbsorption: 0.03,
            spinDecayPerSecond: 0.08,
            restitution: 0.62);

        public static readonly EraProfile Modern = new EraProfile(
            Era.Modern,
            ballMassKg: 0.42,
            dragCoefficient: 0.22,
            liftSlope: 0.95,
            liftMax: 0.32,
            knuckleCoefficient: 0.10,
            knuckleSpinThreshold: 14,
            knuckleSegmentSeconds: 0.12,
            waterAbsorption: 0.01,
            spinDecayPerSecond: 0.08,
            restitution: 0.65);

        public EraProfile(
            Era era,
            double ballMassKg,
            double dragCoefficient,
            double liftSlope,
            double liftMax,
            double knuckleCoefficient,
            double knuckleSpinThreshold,
            double knuckleSegmentSeconds,
            double waterAbsorption,
            double spinDecayPerSecond,
            double restitution)
        {
            if (ballMassKg <= 0) throw new ArgumentOutOfRangeException(nameof(ballMassKg));
            if (knuckleSpinThreshold <= 0) throw new ArgumentOutOfRangeException(nameof(knuckleSpinThreshold));
            if (knuckleSegmentSeconds <= 0) throw new ArgumentOutOfRangeException(nameof(knuckleSegmentSeconds));

            Era = era;
            BallMassKg = ballMassKg;
            DragCoefficient = dragCoefficient;
            LiftSlope = liftSlope;
            LiftMax = liftMax;
            KnuckleCoefficient = knuckleCoefficient;
            KnuckleSpinThreshold = knuckleSpinThreshold;
            KnuckleSegmentSeconds = knuckleSegmentSeconds;
            WaterAbsorption = waterAbsorption;
            SpinDecayPerSecond = spinDecayPerSecond;
            Restitution = restitution;
        }

        public Era Era { get; }

        /// <summary>Dry mass.</summary>
        public double BallMassKg { get; }

        public double DragCoefficient { get; }

        /// <summary>Magnus lift coefficient per unit of spin parameter (radius × spin rate ÷ airspeed).</summary>
        public double LiftSlope { get; }

        public double LiftMax { get; }

        /// <summary>Size of the random side force on a ball struck with almost no spin.</summary>
        public double KnuckleCoefficient { get; }

        /// <summary>Spin rate in rad/s above which the ball stops knuckling.</summary>
        public double KnuckleSpinThreshold { get; }

        /// <summary>How long the knuckle force keeps one direction before drifting to the next.</summary>
        public double KnuckleSegmentSeconds { get; }

        /// <summary>Fraction of extra mass the ball gains in heavy rain.</summary>
        public double WaterAbsorption { get; }

        public double SpinDecayPerSecond { get; }

        /// <summary>Vertical bounciness off the grass.</summary>
        public double Restitution { get; }

        public static EraProfile For(Era era)
        {
            switch (era)
            {
                case Era.Seventies: return Seventies;
                case Era.Nineties: return Nineties;
                case Era.Modern: return Modern;
                default: throw new ArgumentOutOfRangeException(nameof(era));
            }
        }
    }

    /// <summary>Weather and stadium for a match. Both phones must use the same values.</summary>
    public sealed class MatchConditions
    {
        public const double SeaLevelAirDensity = 1.225;
        const double DensityScaleHeightMetres = 8434;

        public static readonly MatchConditions Calm = new MatchConditions(Vec3.Zero, 0, 0);

        public MatchConditions(Vec3 wind, double rain, double altitudeMetres)
        {
            if (rain < 0 || rain > 1) throw new ArgumentOutOfRangeException(nameof(rain), "Rain is 0 (dry) to 1 (monsoon).");
            if (altitudeMetres < -500 || altitudeMetres > 6000) throw new ArgumentOutOfRangeException(nameof(altitudeMetres));
            Wind = wind;
            Rain = rain;
            AltitudeMetres = altitudeMetres;
        }

        /// <summary>Wind velocity in m/s. Wind along +Z blows from the kicker towards the goal.</summary>
        public Vec3 Wind { get; }

        /// <summary>0 for dry, 1 for a monsoon.</summary>
        public double Rain { get; }

        /// <summary>Stadium altitude. Thinner air means less drag and less swerve, so the ball flies further.</summary>
        public double AltitudeMetres { get; }

        public double AirDensity => SeaLevelAirDensity * System.Math.Exp(-AltitudeMetres / DensityScaleHeightMetres);
    }
}
