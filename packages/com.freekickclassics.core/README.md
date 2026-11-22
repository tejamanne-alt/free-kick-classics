# Free Kick Classics Core

Game logic with no `UnityEngine` reference. Depends on `com.freekickclassics.duellink`.

- `Shootout/ShootoutSession`: one Stare-Down Shootout between two phones over an `IDuelLink`, with commit-reveal picks, verified results and replay-based reconnects.
- `Shootout/PenaltyResolver` and `ShootoutTuning`: whole-number penalty rules both phones resolve identically. See [docs/stare-down-rules.md](../../docs/stare-down-rules.md).
- `Shootout/CpuKeeper`, `Tendencies` and `ShootoutScore`: the vs-CPU keeper, the "went left 4 of 5" readout, and shootout scoring with sudden death.
- `Shootout/PenaltyKickPlanner`: turns a resolved kick into a launch that flies through the agreed point.
- `Physics/`: the fixed-timestep flight model (drag, Magnus, knuckle wobble) with 1970s, 1990s and modern balls, wind, rain and altitude, plus the aim solver, the goal frame and the reconciler that keeps the guest's replay on the host's end point.

Axes match Unity: X right, Y up, Z from the kicker towards the goal.
