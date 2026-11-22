namespace FreeKickClassics.Core.Physics
{
    /// <summary>
    /// Real pitch measurements in metres. Penalty frame: the ball sits on the spot at the origin and the goal line is
    /// the plane z = <see cref="PenaltyDistance"/>, centred on x = 0.
    /// </summary>
    public static class Pitch
    {
        public const double BallRadius = 0.11;

        /// <summary>Distance from the centre of the goal to the inside edge of either post.</summary>
        public const double GoalHalfWidth = 3.66;

        /// <summary>Height of the underside of the crossbar.</summary>
        public const double GoalHeight = 2.44;

        /// <summary>Posts and crossbar are 12 cm thick.</summary>
        public const double PostRadius = 0.06;

        public const double PenaltyDistance = 11.0;

        /// <summary>Minimum distance of the wall from the ball: the 9.15 m spray line.</summary>
        public const double WallDistance = 9.15;

        public static Vec3 PenaltySpot => new Vec3(0, BallRadius, 0);
    }

    public enum GoalPlaneZone : byte
    {
        OnTarget = 0,
        Woodwork = 1,
        OffTarget = 2,
    }

    public static class GoalFrame
    {
        const double PostAxisX = Pitch.GoalHalfWidth + Pitch.PostRadius;
        const double BarAxisY = Pitch.GoalHeight + Pitch.PostRadius;
        const double ContactDistance = Pitch.PostRadius + Pitch.BallRadius;

        /// <summary>
        /// Where a ball whose centre crosses the goal-line plane at (x, y) ends up: clean inside the frame, touching
        /// a post or the bar, or off target.
        /// </summary>
        public static GoalPlaneZone Classify(double x, double y)
        {
            double ax = System.Math.Abs(x);

            double postDx = ax - PostAxisX;
            double postDy = y > BarAxisY ? y - BarAxisY : 0;
            double postDistanceSq = postDx * postDx + postDy * postDy;

            double barDx = ax > PostAxisX ? ax - PostAxisX : 0;
            double barDy = y - BarAxisY;
            double barDistanceSq = barDx * barDx + barDy * barDy;

            double contactSq = ContactDistance * ContactDistance;
            if (postDistanceSq <= contactSq || barDistanceSq <= contactSq) return GoalPlaneZone.Woodwork;
            return ax < PostAxisX && y < BarAxisY ? GoalPlaneZone.OnTarget : GoalPlaneZone.OffTarget;
        }
    }
}
