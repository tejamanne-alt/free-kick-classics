# Free Kick Classics — Game Concept

Nov 22, 2026 · @Teja Manne

## The pitch

Free Kick Classics is a football game with only free kicks and penalties. Every match is a duel, kicker against keeper or attacker against wall, played face to face on two phones over Bluetooth, across Android and iOS.

## Why two phones

Two phones in one room are the only setup that keeps both halves of a penalty: a hidden choice and a face to read. A penalty is a mind game, where both players choose in secret at the same moment.

- **One shared phone** doesn't work, because the keeper can see where you're aiming.
- **Online play** works, but you lose the part where you stare down your friend.
- **Two phones in the same room** give you both. Your choice stays hidden on your screen, and you can still watch your friend's face.

So Bluetooth is the reason this game exists, not an extra feature. Free kicks are also turn-based and send very little data, which is what Bluetooth Low Energy (BLE) handles well.

## Modes

The modes come in three groups: versus and co-op over Bluetooth for when friends are together, and solo for the time in between.

### Versus over Bluetooth

- **Stare-Down Shootout.** The kicker secretly picks a spot (9 zones), a strike type and power. The keeper secretly picks a dive side and timing: early, late or stay big. During the 1.5-second run-up either player can bluff. The kicker can stutter, which costs accuracy. The keeper can shuffle on the line, which shrinks the kicker's sweet spot. After each kick, both players see the other's tendencies ("went left 4 of 5"), so the mind games build through the shootout.
- **Wall Wars.** A free-kick duel. The defender secretly sets the wall size, whether the wall jumps or stays, a "lying man" behind the wall and where the keeper starts. The attacker can see the wall but not the jump call. Players swap roles each round.

### Co-op over Bluetooth (both of you against the CPU)

- **Wall & Keeper.** One player moves the wall and times its jump; the other plays keeper. It follows real football logic: the wall covers the near post and the keeper takes the far one. If you don't talk to each other, you concede.
- **Training-Ground Routines.** Two-player set pieces like the dummy run-over, the square roll-and-strike and the lay-off. Each needs timed inputs across both phones.
- **Tag-Team Shootout.** You take turns kicking against a CPU keeper that learns your combined tendencies, so you have to plan together to stay unpredictable.

### Solo

- **Classics campaign.** 50+ scenario cards that recreate the situations behind famous set pieces, with original names and no real players:
  - "The Impossible Banana": 35 m out, the shot has to start wide of the post.
  - "Ninety-Third Minute": one kick, a wall of six, score or get relegated.
  - "The Ice-Cold Chip": the keeper always dives early.
  - "Monsoon Final": a wet, skidding ball.
- **Daily Dead Ball.** One scenario a day, the same for everyone, with a result you can share.

## Mechanics only a free-kick game can have

The depth comes from how you strike the ball, which era you play in, and what the wall and the weather do.

- **Contact-point striking.** You tap where your foot meets the ball, then swipe. The side gives curl, low centre gives a knuckleball with a late wobble, the top gives dip, the outside of the boot gives a trivela, and underneath gives a chip.
- **Era physics, which is what makes it "Classics".** The same kick plays three different ways depending on the era (table below).
- **The wall.** The 9.15 m spray line, walls creeping forward until the referee pushes them back, players who jump and players who stay, and the lying man.
- **Conditions.** Wind, rain, night games, and a high-altitude stadium where the ball flies further.

| Era | Ball | How it plays | Presentation |
| --- | --- | --- | --- |
| 1970s | Leather | Heavy, barely swerves, gets heavier in rain | Grainy TV look |
| 1990s | Glossy | Big bend | 1990s broadcast graphics |
| Modern | Light | Wild knuckleballs | Modern broadcast look |

## The Bluetooth part: what actually works

Real Android-to-iOS Bluetooth means writing a raw BLE GATT layer, because Google's Nearby Connections only gives same-Wi-Fi cross-play on iOS today.

&#91;embedded content: one kick over BLE · 5 steps across host and guest\]

Each kick swaps a hash and a reveal both ways, then the host sends the result one way, so both phones show the same replay.

- **Nearby Connections falls short.** It looks like the obvious choice, but its iOS build [lists Wi-Fi LAN as the only supported medium](https://www.github.com/brian-doherty/nearby). On [the BLE tracking issue](https://github.com/google/nearby/issues/1619), a maintainer said BLE isn't enabled by default for public builds yet, and Monzo reported that iOS-to-Android BLE connections didn't seem possible.
- **Raw BLE GATT.** The host phone is the peripheral and advertises a room code. The guest phone connects as the central. On iOS that's CoreBluetooth; on Android it's the Bluetooth LE API, connected to Unity through a plugin or your own thin native bridge.
- **The host runs the physics.** Unity physics can give slightly different results on different phone chips, so the host simulates the ball's flight and sends the guest the launch values and the result, a few packets in all. The guest runs the same flight model and lands on the host's end point. A kick input is about 20 bytes, which is one BLE write.
- **Commit-reveal for the secret picks.** Each phone first sends a hash of its choice, then reveals the choice. This stops a hacked phone from peeking at the other player's pick, and it costs almost nothing.
- **A transport layer with fallbacks.** Bluetooth first, then same-Wi-Fi, then online later. Make the permission screens clear: Android 12+ asks for "Nearby devices" and iOS shows its own Bluetooth prompt.
- **Build this game before Dunk Duel and Volley Duel.** They need the same Android-to-iOS Bluetooth layer, but they're real-time, so Bluetooth lag will hurt them. Free kicks are turn-based, which makes this the easiest game to build and test one shared Bluetooth module in, then reuse it.

## Tech stack

The game runs on Unity 6.3 LTS in C#. The only native code is the Bluetooth layer, because Unity has no built-in Bluetooth API. Unity 6.3 is the [latest LTS, supported until December 2027](https://www.unity.com/releases); 6.0 LTS support ended in October 2026.

| Layer | Choice | Notes |
| --- | --- | --- |
| Engine | Unity 6.3 LTS, C# | One codebase for Android and iOS |
| Rendering | Universal Render Pipeline (URP) | Mobile-friendly; era looks come from post-processing and UI overlays |
| 3D art and animation | Blender, exported as FBX | Stadiums, players, ball; kick, dive and wall-jump animations |
| Camera and replays | Cinemachine + Timeline | Behind-the-ball camera, keeper camera, slow-motion replays |
| Input | Unity Input System | Tap the contact point, swipe for power and curve |
| Ball physics | Custom C# flight model on a fixed timestep | Drag, spin and knuckle wobble tuned per era; Unity physics only for hits on the wall, keeper and posts |
| CPU keeper and wall | C# state machines + a tendency tracker | The keeper reads your shot history, as in Tag-Team Shootout |
| Bluetooth | [Bluetooth Networking for iOS, tvOS and Android](https://assetstore.unity.com/packages/tools/network/bluetooth-networking-for-ios-tvos-and-android-124274) for the spike; your own plugin (Swift CoreBluetooth, Kotlin BLE) if it falls short | Behind one `IDuelLink` interface |
| Fallbacks | Same-Wi-Fi via .NET UDP sockets; online later via Unity Gaming Services | Same `IDuelLink` interface |
| Services | Unity Gaming Services: Leaderboards, Cloud Save, Remote Config, Analytics | Daily Dead Ball is seeded from the date, so it works offline |
| Payments | Unity IAP | Cosmetics only, no stat upgrades |
| Replay export | A runtime screen-recording plugin | Unity Recorder only works in the editor, not in builds |
| Testing | Unity Test Framework + a phone test matrix | Same inputs must land in the same spot; Bluetooth tested on iPhone, Samsung, Xiaomi and a cheap Android |
| Builds | Xcode on the Mac for iOS signing; Unity Build Automation or GitHub Actions for CI | Android and iOS builds from one branch |

Check the Bluetooth plugin first. Its listing says it connects iOS, tvOS and Android devices, but throughput is only about 8–10 packets per second. It was last updated in July 2024 (v2.4), so confirm it runs on Unity 6.3 during the spike. With that packet budget, each kick sends the launch values and the host's result, not a full trajectory.

## Stress test

The biggest risks are weak solo play, a large online competitor, and a name that is almost taken.

1. **Bluetooth only works when you're in the same room.** Most play sessions will be solo. If Classics and the daily challenge aren't fun on their own, players leave between meetups. Co-op gets people in; solo keeps them.
2. **There's a big existing competitor.** [Football Strike](https://apps.apple.com/us/app/football-strike/id1240162700) offers multiplayer free-kick face-offs with licensed clubs such as FC Barcelona. It has a 4.7 rating from about 387K ratings and needs an internet connection. Don't compete on licences. Compete on face-to-face duels, playing without internet and no pay-to-win. Some of its reviews complain about bot opponents and connection drops. Sell only cosmetics, because paid stat upgrades would ruin duels between friends.
3. **The name is almost taken.** Famobi's [Free Kick Classic (3D Free Kick)](https://www.crazygames.com/game/free-kick-classic) came out in June 2018 and is in browsers and on Android. One letter of difference will lose you store searches and could raise a trademark question; check that properly with someone qualified. You could keep the name with a strong subtitle, or call it **Dead Ball Duel** so it sits alongside Dunk Duel and Volley Duel.
4. **Real history is a licensing risk.** Recreate the situation, never the actual player, club, kit or tournament.
5. **It could get repetitive.** The game only has two actions, shoot and save. The era physics, scenario cards and reading each other's tendencies are what stop it feeling like Football Strike without servers.

**Built-in growth:** every Bluetooth match needs a second install, so each match brings in a new player. That fits online-only marketing, and slow-motion replay export for Reels and Shorts adds to it.

## MVP plan

Test the Bluetooth link first; if it fails, the rest of the plan changes.

1. **Weeks 1–2, test the risky part.** A BLE link between an iPhone and a cheap Android, plus a Xiaomi or Samsung. Send kick packets, and measure round-trip time and reconnects.
2. **MVP.** Stare-Down Shootout over Bluetooth, a vs-CPU keeper, 15 Classics cards, one stadium and one era.
3. **v1.** Wall Wars, the three co-op modes, all three eras, cosmetics and replay export.

## Sources

- [google/nearby issue #1619: BLE support on iOS](https://github.com/google/nearby/issues/1619)
- [Nearby Connections README (iOS build limitation)](https://www.github.com/brian-doherty/nearby)
- [Google Nearby overview](https://developers.google.com/nearby)
- [Football Strike on the App Store](https://apps.apple.com/us/app/football-strike/id1240162700)
- [Free Kick Classic (3D Free Kick) on CrazyGames](https://www.crazygames.com/game/free-kick-classic)

* [Unity releases (LTS support dates)](https://www.unity.com/releases)
* [Bluetooth Networking for iOS, tvOS and Android (Unity Asset Store)](https://assetstore.unity.com/packages/tools/network/bluetooth-networking-for-ios-tvos-and-android-124274)
