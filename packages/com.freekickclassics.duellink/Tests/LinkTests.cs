using System;
using System.Collections.Generic;
using FreeKickClassics.DuelLink.Diagnostics;
using FreeKickClassics.DuelLink.Discovery;
using FreeKickClassics.DuelLink.Wire;
using NUnit.Framework;

namespace FreeKickClassics.DuelLink.Tests
{
    public class LinkMessagesTests
    {
        const int BleDefaultPayload = 20;

        [Test]
        public void EveryMessageFitsOneDefaultBleWrite()
        {
            Assert.That(HelloMessage.Size, Is.LessThanOrEqualTo(BleDefaultPayload));
            Assert.That(CommitMessage.Size, Is.LessThanOrEqualTo(BleDefaultPayload));
            Assert.That(RevealMessage.Size, Is.LessThanOrEqualTo(BleDefaultPayload));
            Assert.That(PingMessage.Size, Is.LessThanOrEqualTo(BleDefaultPayload));
            Assert.That(AbortMessage.Size, Is.LessThanOrEqualTo(BleDefaultPayload));
        }

        [Test]
        public void RoundTrips()
        {
            var hello = HelloMessage.Decode(new HelloMessage(1, 7, DuelRole.Guest, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Encode());
            Assert.That(hello.ProtocolVersion, Is.EqualTo(1));
            Assert.That(hello.GameId, Is.EqualTo(7));
            Assert.That(hello.Role, Is.EqualTo(DuelRole.Guest));
            Assert.That(hello.Nonce, Is.EqualTo(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }));

            var commitment = new byte[16];
            commitment[15] = 0xAB;
            var commit = CommitMessage.Decode(new CommitMessage(200, commitment).Encode());
            Assert.That(commit.Round, Is.EqualTo(200));
            Assert.That(commit.Commitment, Is.EqualTo(commitment));

            var nonce = new byte[12];
            nonce[0] = 0xCD;
            var reveal = RevealMessage.Decode(new RevealMessage(5, new byte[] { 1, 2, 3, 4 }, nonce).Encode());
            Assert.That(reveal.Round, Is.EqualTo(5));
            Assert.That(reveal.Payload, Is.EqualTo(new byte[] { 1, 2, 3, 4 }));
            Assert.That(reveal.Nonce, Is.EqualTo(nonce));

            var pong = PingMessage.Decode(new PingMessage(true, 65000).Encode());
            Assert.That(pong.IsPong, Is.True);
            Assert.That(pong.Sequence, Is.EqualTo(65000));

            Assert.That(AbortMessage.Decode(new AbortMessage(9).Encode()).Reason, Is.EqualTo(9));
        }

        [Test]
        public void RejectsMalformedMessages()
        {
            var commit = new CommitMessage(0, new byte[16]).Encode();
            Assert.Throws<ProtocolException>(() => CommitMessage.Decode(new byte[0]));
            Assert.Throws<ProtocolException>(() => CommitMessage.Decode(Truncate(commit)));
            Assert.Throws<ProtocolException>(() => RevealMessage.Decode(commit));

            var hello = new HelloMessage(1, 1, DuelRole.Host, new byte[8]).Encode();
            hello[3] = 2;
            Assert.Throws<ProtocolException>(() => HelloMessage.Decode(hello));
        }

        [TestCase(5, 5, 0)]
        [TestCase(4, 5, -1)]
        [TestCase(6, 5, 1)]
        [TestCase(255, 256, -1)]
        [TestCase(0, 256, 0)]
        [TestCase(0, 255, 1)]
        public void PlacesWrappedRoundNumbers(int received, int local, int expected)
        {
            Assert.That(WireChecks.RoundDelta((byte)received, local), Is.EqualTo(expected));
        }

        static byte[] Truncate(byte[] data)
        {
            var shorter = new byte[data.Length - 1];
            Array.Copy(data, shorter, shorter.Length);
            return shorter;
        }
    }

    public class LoopbackHubTests
    {
        [Test]
        public void DeliversInOrderOnlyWhenPumped()
        {
            var hub = new LoopbackHub();
            hub.Connect();
            var received = new List<byte>();
            hub.B.MessageReceived += data => received.Add(data[0]);

            hub.A.Send(new byte[] { 1 });
            hub.A.Send(new byte[] { 2 });
            Assert.That(received, Is.Empty);
            hub.DeliverAll();
            Assert.That(received, Is.EqualTo(new byte[] { 1, 2 }));
        }

        [Test]
        public void RefusesMessagesLargerThanTheLinkCarries()
        {
            var hub = new LoopbackHub(maxMessageSize: 20);
            hub.Connect();
            Assert.Throws<ArgumentException>(() => hub.A.Send(new byte[21]));
            Assert.DoesNotThrow(() => hub.A.Send(new byte[20]));
        }

        [Test]
        public void DropsSendsWhileDisconnectedAndMessagesInFlight()
        {
            var hub = new LoopbackHub();
            int received = 0;
            hub.B.MessageReceived += _ => received++;

            hub.A.Send(new byte[] { 1 });
            Assert.That(hub.A.DroppedWhileDisconnected, Is.EqualTo(1));

            hub.Connect();
            hub.A.Send(new byte[] { 2 });
            hub.Disconnect();
            hub.Connect();
            hub.DeliverAll();
            Assert.That(received, Is.EqualTo(0));
        }

        [Test]
        public void InterceptCanTamperOrDrop()
        {
            var hub = new LoopbackHub();
            hub.Connect();
            var received = new List<byte>();
            hub.B.MessageReceived += data => received.Add(data[0]);
            hub.Intercept = (from, data) => data[0] == 1 ? null : new byte[] { (byte)(data[0] * 10) };

            hub.A.Send(new byte[] { 1 });
            hub.A.Send(new byte[] { 2 });
            hub.DeliverAll();
            Assert.That(received, Is.EqualTo(new byte[] { 20 }));
        }
    }

    public class RttProbeTests
    {
        [Test]
        public void MeasuresRoundTripsAndCountsLosses()
        {
            var hub = new LoopbackHub();
            hub.Connect();
            double now = 0;
            var probe = new RttProbe(hub.A, () => now);
            var echo = new RttProbe(hub.B, () => now);

            for (int i = 0; i < 10; i++)
            {
                probe.SendPing();
                now += 0.010; // ping in flight
                hub.DeliverNext();
                now += 0.005 * i; // pong in flight
                hub.DeliverNext();
            }

            hub.Intercept = (from, data) => null;
            probe.SendPing();

            var stats = probe.GetStats();
            Assert.That(stats.Count, Is.EqualTo(10));
            Assert.That(stats.Lost, Is.EqualTo(1));
            Assert.That(stats.Min, Is.EqualTo(0.010).Within(1e-9));
            Assert.That(stats.Max, Is.EqualTo(0.055).Within(1e-9));
            Assert.That(stats.Median, Is.EqualTo(0.030).Within(1e-9));
            Assert.That(stats.P95, Is.EqualTo(0.055).Within(1e-9));
            Assert.That(echo.Sent, Is.EqualTo(0));
            Assert.That(stats.ToString(), Does.Contain("median=30.0ms"));
        }
    }

    public class RoomAdvertisementTests
    {
        [Test]
        public void RoundTripsGameVersionAndRoomCode()
        {
            string uuid = RoomAdvertisement.ServiceUuid(gameId: 1, protocolVersion: 3, roomCode: 427);
            Assert.That(uuid, Is.EqualTo("c6eb9b26-a6da-4739-889b-6fa4010301ab"));
            Assert.That(Guid.TryParse(uuid, out _), Is.True);

            Assert.That(RoomAdvertisement.TryParse(uuid.ToUpperInvariant(), out byte game, out byte version, out int code), Is.True);
            Assert.That(game, Is.EqualTo(1));
            Assert.That(version, Is.EqualTo(3));
            Assert.That(code, Is.EqualTo(427));
            Assert.That(RoomAdvertisement.FormatRoomCode(code), Is.EqualTo("0427"));
        }

        [Test]
        public void RejectsForeignAndOutOfRangeUuids()
        {
            Assert.That(RoomAdvertisement.TryParse("0000180d-0000-1000-8000-00805f9b34fb", out _, out _, out _), Is.False);
            Assert.That(RoomAdvertisement.TryParse("c6eb9b26-a6da-4739-889b-6fa40101ffff", out _, out _, out _), Is.False);
            Assert.Throws<ArgumentOutOfRangeException>(() => RoomAdvertisement.ServiceUuid(1, 1, 10000));
        }
    }
}
