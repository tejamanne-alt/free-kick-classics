using System;
using FreeKickClassics.DuelLink;

namespace FreeKickClassics.Core.Shootout
{
    /// <summary>
    /// Penalty shootout scoring: players alternate kicks, five each by default, it ends as soon as one side can't
    /// catch up, and a tie after the regulation kicks goes to sudden death in pairs.
    /// </summary>
    public sealed class ShootoutScore
    {
        readonly int[] goals = new int[2];
        readonly int[] kicks = new int[2];

        public ShootoutScore(DuelRole firstKicker, int kicksPerSide = 5)
        {
            if (kicksPerSide < 1) throw new ArgumentOutOfRangeException(nameof(kicksPerSide));
            FirstKicker = firstKicker;
            KicksPerSide = kicksPerSide;
        }

        public DuelRole FirstKicker { get; }

        public int KicksPerSide { get; }

        public bool IsDecided { get; private set; }

        /// <summary>Only meaningful once <see cref="IsDecided"/>.</summary>
        public DuelRole Winner { get; private set; }

        public bool InSuddenDeath => Math.Min(kicks[0], kicks[1]) >= KicksPerSide && !IsDecided;

        /// <summary>Who takes the next kick.</summary>
        public DuelRole NextKicker => kicks[(int)FirstKicker] == kicks[(int)FirstKicker.Other()] ? FirstKicker : FirstKicker.Other();

        public int Goals(DuelRole role) => goals[(int)role];

        public int Kicks(DuelRole role) => kicks[(int)role];

        public void Record(DuelRole kicker, bool scored)
        {
            if (IsDecided) throw new InvalidOperationException("The shootout is already decided.");
            if (kicker != NextKicker) throw new InvalidOperationException(kicker + " kicked out of turn; " + NextKicker + " was next.");

            kicks[(int)kicker]++;
            if (scored) goals[(int)kicker]++;
            Evaluate();
        }

        void Evaluate()
        {
            int a = (int)FirstKicker;
            int b = (int)FirstKicker.Other();

            if (Math.Min(kicks[a], kicks[b]) >= KicksPerSide)
            {
                // Regulation is over: only a completed pair can settle it.
                if (kicks[a] == kicks[b] && goals[a] != goals[b]) Decide(goals[a] > goals[b] ? a : b);
                return;
            }

            int remainingA = Math.Max(0, KicksPerSide - kicks[a]);
            int remainingB = Math.Max(0, KicksPerSide - kicks[b]);
            if (goals[a] > goals[b] + remainingB) Decide(a);
            else if (goals[b] > goals[a] + remainingA) Decide(b);
        }

        void Decide(int winner)
        {
            IsDecided = true;
            Winner = (DuelRole)winner;
        }
    }
}
