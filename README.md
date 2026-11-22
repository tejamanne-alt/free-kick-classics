# Free Kick Classics

A football game with only free kicks and penalties, played face to face on two phones over Bluetooth, across Android and iOS. See the [game concept](docs/concept.md).

This repository holds the engine-agnostic core so far: the shared Bluetooth duel layer and the Stare-Down Shootout logic, as two Unity packages in plain C# with tests that run without the Unity editor.

## What's here

| Path | What it is |
| --- | --- |
| [`packages/com.freekickclassics.duellink`](packages/com.freekickclassics.duellink) | **Duel Link**, the reusable link layer for Dunk Duel and Volley Duel too: the `IDuelLink` interface, messages that each fit one 20-byte BLE write, commit-reveal (self-contained SHA-256), shared fair randomness, room-code advertisement UUIDs, an `RttProbe` for the spike, and `LoopbackHub` for testing two phones in one process |
| [`packages/com.freekickclassics.core`](packages/com.freekickclassics.core) | **Game core:** `ShootoutSession` (a full Stare-Down Shootout between two phones over any `IDuelLink`), the penalty rules and their tuning, shootout scoring, tendencies ("went left 4 of 5"), the CPU keeper, and the ball-flight model with three eras, wind, rain and altitude |
| [`docs/duel-protocol.md`](docs/duel-protocol.md) | Wire protocol, BLE GATT layout, platform pitfalls and the **weeks 1–2 Bluetooth spike plan** |
| [`docs/stare-down-rules.md`](docs/stare-down-rules.md) | How a penalty resolves, the balance guardrail, open design questions |
| `dotnet/` | Build-only projects so the packages compile and test with `dotnet test` (locally and in CI) |

Neither package references `UnityEngine` (`noEngineReferences` is set), so the logic is testable anywhere. The Unity layer converts `Vec3` to `Vector3`, draws the replay and handles input.

## Run the tests

You need the .NET 8 SDK.

```bash
dotnet test FreeKickClassics.sln
```

The same test files run in Unity's Test Runner. They cover:

- **The duel protocol:** a full shootout where both phones agree on every kick, neither phone revealing before it holds the other's commitment, a hacked guest changing its pick, a guest mirroring the host's pick, a host lying about the result, random disconnects mid-kick, and version, game and rules mismatches.
- **Physics:** a vacuum check against the analytic projectile, curl and dip directions, the 1990s ball bending more than the 1970s one, a late knuckle wobble, altitude and rain, bit-for-bit repeatability, and the aim solver hitting every target in every era.
- **Rules:** the outcome matches where the ball goes, the rolls follow the stated probabilities, the football logic holds (a chip beats an early dive, a stutter wrecks one), and the balance guardrail.

## Use it in Unity 6.3

1. Create a URP project in `unity/` with Unity 6.3 LTS. The `.gitignore` already covers `unity/Library` and the rest of Unity's generated folders.
2. Add both packages and mark them testable in `unity/Packages/manifest.json`:

   ```json
   {
     "dependencies": {
       "com.freekickclassics.duellink": "file:../../packages/com.freekickclassics.duellink",
       "com.freekickclassics.core": "file:../../packages/com.freekickclassics.core"
     },
     "testables": ["com.freekickclassics.duellink", "com.freekickclassics.core"]
   }
   ```

   Merge these keys into the generated manifest; don't replace it. Unity writes `.meta` files into the package folders on first import; commit them.
3. Open **Window › General › Test Runner** and run the EditMode tests.

A minimal two-phone hookup looks like this (`bleLink` is your adapter around the Bluetooth plugin):

```csharp
var session = new ShootoutSession(DuelRole.Host, bleLink);
session.RoundStarted += (round, kicker) => ShowPickScreen(session.IsLocalKicker);
session.RemoteLockedIn += round => ShowOpponentLockedIn();
session.RoundCompleted += report => PlayReplay(report.Launch, report.EndPoint, report.Outcome);
session.Start();
// When the player locks in:
session.SubmitKick(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, power: 180));
```

## Where this is against the MVP plan

**Weeks 1–2: Bluetooth spike.** The protocol, room advertisement scheme, RTT probe and test harness are ready, and [docs/duel-protocol.md](docs/duel-protocol.md) has the device matrix, what to measure and a suggested pass bar. Still to do on real phones:

- an `IDuelLink` adapter over the Asset Store Bluetooth plugin, or a thin Swift/Kotlin plugin
- a spike scene
- the measurements

**MVP.** Done in the core:

- Stare-Down Shootout over any link, with commit-reveal, verified results and reconnects
- the CPU keeper
- tendencies
- the flight model with all three eras

Still to do:

- Unity scenes, input, animation and replays
- the contact-point strike model and the wall, for the 15 Classics cards
- one stadium and the era presentation

**v1.** Wall Wars and the co-op modes can reuse `IDuelLink`, the commit-reveal and the flight model. Each needs its own session and rules.

## Decisions worth a look

- **Penalty outcomes come from whole-number rules plus a seed both phones derive, not from collision physics.** The guest can then verify the host instead of trusting it. The host still runs the flight and sends launch values and the end point, as the concept describes.
- **Stutter and shuffle are part of the secret pick**, not live actions during the run-up. See the open questions in [stare-down-rules.md](docs/stare-down-rules.md#open-design-questions).
- **The room code lives in the advertised service UUID**, because that's the only advertising field both iOS and Android let an app set as a peripheral.
