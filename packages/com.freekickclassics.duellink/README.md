# Duel Link

The link between two phones for turn-based duels, kept free of game rules so Free Kick Classics, Dunk Duel and Volley Duel can share it.

- `IDuelLink`: an ordered message link (BLE, same-Wi-Fi or online) that drops messages while disconnected and raises its events on the main thread.
- `Wire/`: Hello, Commit, Reveal, Ping/Pong and Abort, each at most 18 bytes so it fits one 20-byte BLE write. Games add their own types from `0x40`.
- `Crypto/`: SHA-256 in plain C#, commit-reveal bound to session, round and committer, and shared randomness (session id, coin toss, per-round seed).
- `Discovery/RoomAdvertisement`: the advertised service UUID with the room code inside, plus the characteristic UUIDs.
- `Diagnostics/RttProbe`: round-trip-time stats for the Bluetooth spike.
- `LoopbackHub`: two link ends in one process, with manual delivery, disconnects and tampering for tests.

No `UnityEngine` reference. Protocol details: [docs/duel-protocol.md](../../docs/duel-protocol.md).
