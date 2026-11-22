# Stare-Down Shootout rules

How one penalty is decided in `PenaltyResolver`, and how the default numbers in `ShootoutTuning` are kept honest. Everything is whole-number per-mille chances and percent multipliers, so both phones resolve every kick identically.

## The picks

**Kicker:** a zone in the 3×3 grid (columns and rows as the kicker sees them), a strike type, power 0–255, and whether to stutter.

| Strike | Strengths | Weaknesses |
| --- | --- | --- |
| Placed (side-foot) | Most accurate, curls away towards its corner | Easiest to save |
| Driven (laces) | Fastest, hardest to save, knuckles with a modern ball | Misses more |
| Chip | Almost never saved by an early dive | Caught by a keeper who stays big |

**Keeper:** a dive side, a timing, and whether to shuffle on the line.

| Timing | Covers | Beaten by |
| --- | --- | --- |
| Early | The chosen corner, best of all | The other side, a chip, a stutter |
| Late | The chosen side, less far into the corners, plus some of the middle | Corners, especially high ones |
| Stay big | The middle, leaning to the chosen side | Anything placed into a corner |

**Bluffs:** a stutter makes an early dive go too soon, cutting that keeper's save chance to about a third, but raises the kicker's miss chance by 60%. It does nothing to a late dive or a keeper who stays big. A shuffle on the line raises the kicker's miss chance by 35% but trims the keeper's reach by 10%.

## How a kick resolves

1. **Miss chance** = base miss for the strike × zone risk × power × stutter × shuffle, capped at 60%. The low and middle centre zones can't miss. Top corners are the riskiest.
2. **Save chance** (if on target) = coverage for this timing, the zone's column relative to the dive (same side, centre or far side) and its row, × strike × power × chip-against-timing × stutter-against-early × shuffle, capped at 95%.
3. **One roll from the shared seed** decides miss or on target. A miss hits the post or bar 22% of the time; otherwise it goes wide, or over for the centre column and sometimes from the top corners. An on-target ball scatters inside its zone (wider for driven shots), then a second roll decides save or goal.
4. The resolver returns the outcome and the point on the goal line where the ball crosses. The host's `PenaltyKickPlanner` then solves for a launch that sends the ball through exactly that point under the era's physics, including curl and knuckle wobble.

## Balance guardrail

`PenaltyBalanceTests` solves the kicker-versus-keeper game with fictitious play and checks two things about the defaults:

- **The equilibrium goal rate is 70–80%**, close to real penalty conversion (about 75%). The defaults give **74.4%**.
- **Every option is worth picking.** For each row, column, strike type, power band and stutter, the best kick using it is within 3 points of optimal against an optimal keeper. The same goes for each keeper timing and the shuffle. With the defaults, every option is within 2 points.

Optimal play itself is sparse: a perfect kicker mixes only a handful of kicks. That's normal for games like this. The fun comes from players drifting away from optimal and reading each other's tendencies, which is what the after-kick "went left 4 of 5" readout (`TendencyReader`) and the CPU keeper (`CpuKeeper`) feed on.

Change any number in `ShootoutTuning` and the test tells you whether the game is still a mind game. Both phones must run the same numbers: the host sends `ShootoutTuning.Fingerprint()` in `MatchSetup`, and a guest with different rules refuses to play.

## Open design questions

- **Live bluffs.** The concept has players bluff *during* the 1.5-second run-up. Here, stutter and shuffle are part of the secret pick and revealed with it. Shown live, a bluff can't change a pick that's already locked in, and sending it in real time would undo the commit-reveal guarantee. A live, cosmetic "feint" animation could still be added on top.
- **Keeper height.** Keepers pick a side and a timing, not a height; height comes in through the per-row coverage. A "dive high / dive low" pick would make the grid fully 3×3 against 3×3.
- **Power.** Right now power is a single trade-off of speed against accuracy. A swipe-strength input could feed it directly.
