using System;
using System.Collections.Generic;
using System.Linq;
using FreeKickClassics.Core.Physics;
using FreeKickClassics.Core.Shootout;
using FreeKickClassics.DuelLink;
using FreeKickClassics.DuelLink.Crypto;
using FreeKickClassics.DuelLink.Wire;
using NUnit.Framework;

namespace FreeKickClassics.Core.Tests
{
    /// <summary>Two sessions on a loopback link, with players that pick at random as soon as a round starts.</summary>
    sealed class Match
    {
        public readonly LoopbackHub Hub = new LoopbackHub();
        public readonly ShootoutSession Host;
        public readonly ShootoutSession Guest;
        public readonly List<RoundReport> HostReports = new List<RoundReport>();
        public readonly List<RoundReport> GuestReports = new List<RoundReport>();
        public readonly List<(DuelRole Role, int Round)> RemoteLockIns = new List<(DuelRole, int)>();

        readonly DeterministicRandom hostPlayer;
        readonly DeterministicRandom guestPlayer;

        public Match(ulong seed, ShootoutSessionOptions hostOptions = null, ShootoutSessionOptions guestOptions = null, bool autoPlay = true)
        {
            hostPlayer = new DeterministicRandom(seed * 2 + 1);
            guestPlayer = new DeterministicRandom(seed * 2 + 2);
            hostOptions ??= new ShootoutSessionOptions();
            guestOptions ??= new ShootoutSessionOptions();
            hostOptions.Nonces ??= new InsecureSeededNonceSource(seed * 31 + 1);
            guestOptions.Nonces ??= new InsecureSeededNonceSource(seed * 31 + 2);

            Host = new ShootoutSession(DuelRole.Host, Hub.A, hostOptions);
            Guest = new ShootoutSession(DuelRole.Guest, Hub.B, guestOptions);
            Host.RoundCompleted += HostReports.Add;
            Guest.RoundCompleted += GuestReports.Add;
            Host.RemoteLockedIn += round => RemoteLockIns.Add((DuelRole.Host, round));
            Guest.RemoteLockedIn += round => RemoteLockIns.Add((DuelRole.Guest, round));

            if (autoPlay)
            {
                Host.RoundStarted += (round, kicker) => Pick(Host, hostPlayer);
                Guest.RoundStarted += (round, kicker) => Pick(Guest, guestPlayer);
            }
        }

        public static void Pick(ShootoutSession session, DeterministicRandom player)
        {
            if (session.IsLocalKicker) session.SubmitKick(Picks.RandomKick(player));
            else session.SubmitSave(Picks.RandomSave(player));
        }

        public Match Run()
        {
            Hub.Connect();
            Host.Start();
            Guest.Start();
            Hub.DeliverAll();
            return this;
        }
    }

    public class ShootoutSessionTests
    {
        [Test]
        public void TwoPhonesPlayAFullShootoutAndAgreeOnEveryKick([Range(0, 24)] int seed)
        {
            var match = new Match((ulong)seed).Run();

            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Finished));
            Assert.That(match.Guest.Phase, Is.EqualTo(SessionPhase.Finished));
            Assert.That(match.Guest.Score.Winner, Is.EqualTo(match.Host.Score.Winner));
            Assert.That(match.HostReports.Count, Is.EqualTo(match.GuestReports.Count));
            Assert.That(match.HostReports.Count, Is.GreaterThanOrEqualTo(6));

            for (int i = 0; i < match.HostReports.Count; i++)
            {
                var host = match.HostReports[i];
                var guest = match.GuestReports[i];
                Assert.That(guest.Kick, Is.EqualTo(host.Kick));
                Assert.That(guest.Save, Is.EqualTo(host.Save));
                Assert.That(guest.Resolution, Is.EqualTo(host.Resolution));
                Assert.That(guest.Launch.Velocity, Is.EqualTo(host.Launch.Velocity));
                Assert.That(guest.Launch.Spin, Is.EqualTo(host.Launch.Spin));
                Assert.That(guest.Launch.KnuckleSeed, Is.EqualTo(host.Launch.KnuckleSeed));
                Assert.That((guest.EndPoint - host.EndPoint).Length, Is.LessThan(0.001));
                Assert.That(guest.HostGoals, Is.EqualTo(host.HostGoals));

                // The guest's own replay of the launch it was sent lands where the host said.
                var replay = Flight.Simulate(match.Guest.FlightModel, guest.Launch, Pitch.PenaltyDistance);
                Assert.That((replay.EndPoint - guest.EndPoint).Length, Is.LessThan(0.001));
            }

            Assert.That(match.Host.KicksBy(DuelRole.Host).Count + match.Host.KicksBy(DuelRole.Guest).Count, Is.EqualTo(match.HostReports.Count));
        }

        [Test]
        public void EveryMessageFitsOneDefaultBleWrite()
        {
            var match = new Match(3).Run();
            Assert.That(match.Hub.SentLog.Max(m => m.Data.Length), Is.LessThanOrEqualTo(20));
            Assert.That(match.Hub.A.MaxMessageSize, Is.EqualTo(20));
        }

        [Test]
        public void NeitherPhoneRevealsBeforeHoldingTheOthersCommitment()
        {
            var match = new Match(4).Run();
            var log = match.Hub.SentLog;
            for (int round = 0; round < match.HostReports.Count; round++)
            {
                foreach (var (me, other) in new[] { (match.Hub.A, match.Hub.B), (match.Hub.B, match.Hub.A) })
                {
                    int myReveal = log.FindIndex(m => m.From == me && m.Data[0] == LinkMessageType.Reveal && m.Data[1] == round);
                    int theirCommit = log.FindIndex(m => m.From == other && m.Data[0] == LinkMessageType.Commit && m.Data[1] == round);
                    Assert.That(myReveal, Is.GreaterThan(theirCommit), "round " + round);
                }
            }
        }

        [Test]
        public void TellsEachPlayerWhenTheOtherHasLockedIn()
        {
            var match = new Match(5).Run();
            Assert.That(match.RemoteLockIns.Count(l => l.Role == DuelRole.Host), Is.EqualTo(match.HostReports.Count));
            Assert.That(match.RemoteLockIns.Count(l => l.Role == DuelRole.Guest), Is.EqualTo(match.GuestReports.Count));
        }

        [Test]
        public void AHackedGuestThatChangesItsPickAfterTheRevealIsCaught()
        {
            var match = new Match(6);
            match.Hub.Intercept = (from, data) =>
            {
                if (from == match.Hub.B && data[0] == LinkMessageType.Reveal) data[2] ^= 1;
                return data;
            };
            match.Run();

            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Aborted));
            Assert.That(match.Host.AbortReason, Is.EqualTo(AbortReason.CommitmentMismatch));
            Assert.That(match.Guest.AbortReason, Is.EqualTo(AbortReason.RemoteAborted));
            Assert.That(match.Guest.RemoteAbortReason, Is.EqualTo(AbortReason.CommitmentMismatch));
            Assert.That(match.HostReports, Is.Empty);
        }

        [Test]
        public void AGuestCantMirrorTheHostsPick()
        {
            // A dishonest guest, scripted by hand: it echoes the host's commitment as its own, waits for the host to
            // reveal, then echoes the reveal. Without the committer's role in the hash this would verify, and a keeper
            // could copy the kicker's pick.
            var hub = new LoopbackHub();
            var host = new ShootoutSession(DuelRole.Host, hub.A, new ShootoutSessionOptions { Nonces = new InsecureSeededNonceSource(1) });
            host.RoundStarted += (round, kicker) =>
            {
                if (host.IsLocalKicker) host.SubmitKick(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 100));
                else host.SubmitSave(new KeeperChoice(DiveSide.Left, DiveTiming.Early));
            };
            var fromHost = new List<byte[]>();
            hub.B.MessageReceived += fromHost.Add;

            hub.Connect();
            host.Start();
            hub.B.Send(new HelloMessage(ShootoutSession.ProtocolVersion, ShootoutSession.GameId, DuelRole.Guest, new byte[8]).Encode());
            hub.DeliverAll();

            hub.B.Send((byte[])fromHost.Single(m => m[0] == LinkMessageType.Commit).Clone());
            hub.DeliverAll();
            Assert.That(host.HasRemoteCommitted, Is.True);

            hub.B.Send((byte[])fromHost.Single(m => m[0] == LinkMessageType.Reveal).Clone());
            hub.DeliverAll();
            Assert.That(host.AbortReason, Is.EqualTo(AbortReason.CommitmentMismatch));
        }

        [Test]
        public void AHostThatLiesAboutTheResultIsCaught()
        {
            var match = new Match(8);
            match.Hub.Intercept = (from, data) =>
            {
                if (from == match.Hub.A && data[0] == ShootoutMessageType.Result) data[2] = (byte)(data[2] == 0 ? 1 : 0);
                return data;
            };
            match.Run();

            Assert.That(match.Guest.AbortReason, Is.EqualTo(AbortReason.OutcomeMismatch));
            Assert.That(match.Host.AbortReason, Is.EqualTo(AbortReason.RemoteAborted));
            Assert.That(match.GuestReports, Is.Empty);
        }

        [Test]
        public void AHostThatSendsTheBallSomewhereElseIsCaught()
        {
            var match = new Match(9);
            match.Hub.Intercept = (from, data) =>
            {
                if (from == match.Hub.A && data[0] == ShootoutMessageType.Result) data[4] ^= 0x01; // end x moves 256 mm
                return data;
            };
            match.Run();
            Assert.That(match.Guest.AbortReason, Is.EqualTo(AbortReason.OutcomeMismatch));
        }

        [Test]
        public void SurvivesDroppedConnectionsAtAnyMoment([Range(0, 19)] int seed)
        {
            var match = new Match((ulong)seed);
            var chaos = new DeterministicRandom((ulong)seed + 1000);
            match.Hub.Connect();
            match.Host.Start();
            match.Guest.Start();

            int drops = 0;
            for (int step = 0; step < 100_000 && match.Hub.Pending > 0; step++)
            {
                if (step == 7 || chaos.NextInt(8) == 0)
                {
                    match.Hub.Disconnect();
                    match.Hub.Connect();
                    drops++;
                }
                else
                {
                    match.Hub.DeliverNext();
                }
            }

            Assert.That(drops, Is.GreaterThan(0));
            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Finished), "host");
            Assert.That(match.Guest.Phase, Is.EqualTo(SessionPhase.Finished), "guest");
            Assert.That(match.GuestReports.Select(r => r.Resolution), Is.EqualTo(match.HostReports.Select(r => r.Resolution)));
        }

        [Test]
        public void PicksMadeWhileDisconnectedGoOutOnReconnect()
        {
            var match = new Match(10, autoPlay: false);
            match.Hub.Connect();
            match.Host.Start();
            match.Guest.Start();
            match.Hub.DeliverAll();
            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Playing));
            Assert.That(match.Guest.Phase, Is.EqualTo(SessionPhase.Playing));

            match.Hub.Disconnect();
            var player = new DeterministicRandom(1);
            Match.Pick(match.Host, player);
            Match.Pick(match.Guest, player);
            match.Hub.DeliverAll();
            Assert.That(match.HostReports, Is.Empty);

            match.Hub.Connect();
            match.Hub.DeliverAll();
            Assert.That(match.HostReports.Count, Is.EqualTo(1));
            Assert.That(match.GuestReports.Count, Is.EqualTo(1));
        }

        [Test]
        public void SetupArrivingBeforeTheGuestStartsIsKept()
        {
            var match = new Match(11);
            match.Hub.Connect();
            match.Guest.Start();
            match.Host.Start();
            match.Hub.DeliverAll();
            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Finished));
            Assert.That(match.Guest.Phase, Is.EqualTo(SessionPhase.Finished));
        }

        [Test]
        public void HostSetupDecidesEraWeatherAndLength()
        {
            var tuning = ShootoutTuning.CreateDefault();
            var setup = new MatchSetup(Era.Seventies, 20, -10, 255, 2200, 3, tuning.Fingerprint());
            var match = new Match(12, hostOptions: new ShootoutSessionOptions { Setup = setup }).Run();
            Assert.That(match.Guest.Setup, Is.EqualTo(setup));
            Assert.That(match.Guest.FlightModel.Era.Era, Is.EqualTo(Era.Seventies));
            Assert.That(match.Guest.FlightModel.Conditions.AltitudeMetres, Is.EqualTo(2200));
            Assert.That(match.Guest.Score.KicksPerSide, Is.EqualTo(3));
            Assert.That(match.Guest.Phase, Is.EqualTo(SessionPhase.Finished));
        }

        [Test]
        public void RefusesToPlayWithDifferentRules()
        {
            var tweaked = ShootoutTuning.CreateDefault();
            tweaked.MaxSavePerMille = 999;
            var match = new Match(13, guestOptions: new ShootoutSessionOptions { Tuning = tweaked }).Run();
            Assert.That(match.Guest.AbortReason, Is.EqualTo(AbortReason.RulesMismatch));
            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Aborted));
        }

        [Test]
        public void TwoHostsCantPlayEachOther()
        {
            var hub = new LoopbackHub();
            var a = new ShootoutSession(DuelRole.Host, hub.A, new ShootoutSessionOptions { Nonces = new InsecureSeededNonceSource(1) });
            var b = new ShootoutSession(DuelRole.Host, hub.B, new ShootoutSessionOptions { Nonces = new InsecureSeededNonceSource(2) });
            hub.Connect();
            a.Start();
            b.Start();
            hub.DeliverAll();
            Assert.That(a.AbortReason, Is.EqualTo(AbortReason.RoleConflict));
        }

        [Test]
        public void RejectsANewerProtocolOrAnotherGame()
        {
            Assert.That(PeerSays(new HelloMessage(ShootoutSession.ProtocolVersion + 1, ShootoutSession.GameId, DuelRole.Guest, new byte[8]).Encode()),
                Is.EqualTo(AbortReason.ProtocolVersionMismatch));
            Assert.That(PeerSays(new HelloMessage(ShootoutSession.ProtocolVersion, 0x02, DuelRole.Guest, new byte[8]).Encode()),
                Is.EqualTo(AbortReason.WrongGame));
            Assert.That(PeerSays(new byte[] { 0x7E, 1, 2 }), Is.EqualTo(AbortReason.MalformedMessage));
            Assert.That(PeerSays(new byte[] { LinkMessageType.Commit, 0 }), Is.EqualTo(AbortReason.MalformedMessage));
        }

        [Test]
        public void ARevealWithoutACommitmentIsAProtocolViolation()
        {
            var hub = new LoopbackHub();
            var host = new ShootoutSession(DuelRole.Host, hub.A, new ShootoutSessionOptions { Nonces = new InsecureSeededNonceSource(1) });
            hub.Connect();
            host.Start();
            hub.B.Send(new HelloMessage(ShootoutSession.ProtocolVersion, ShootoutSession.GameId, DuelRole.Guest, new byte[8]).Encode());
            hub.B.Send(new RevealMessage(0, new KeeperChoice(DiveSide.Left, DiveTiming.Early).Encode(), new byte[12]).Encode());
            hub.DeliverAll();
            Assert.That(host.AbortReason, Is.EqualTo(AbortReason.UnexpectedMessage));
        }

        [Test]
        public void GuardsAgainstPickingOutOfTurn()
        {
            var match = new Match(14, autoPlay: false);
            Assert.Throws<InvalidOperationException>(() => match.Host.SubmitKick(new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 1)));
            match.Run();

            var kicker = match.Host.IsLocalKicker ? match.Host : match.Guest;
            var keeper = match.Host.IsLocalKicker ? match.Guest : match.Host;
            var kick = new KickerChoice(GoalColumn.Left, GoalRow.Low, StrikeType.Placed, 1);
            Assert.Throws<InvalidOperationException>(() => keeper.SubmitKick(kick));
            Assert.Throws<InvalidOperationException>(() => kicker.SubmitSave(new KeeperChoice(DiveSide.Left, DiveTiming.Late)));
            kicker.SubmitKick(kick);
            Assert.Throws<InvalidOperationException>(() => kicker.SubmitKick(kick));
        }

        [Test]
        public void QuittingTellsTheOtherPhone()
        {
            var match = new Match(15, autoPlay: false).Run();
            match.Guest.Quit();
            match.Hub.DeliverAll();
            Assert.That(match.Guest.AbortReason, Is.EqualTo(AbortReason.LocalQuit));
            Assert.That(match.Host.AbortReason, Is.EqualTo(AbortReason.RemoteAborted));
            Assert.That(match.Host.RemoteAbortReason, Is.EqualTo(AbortReason.LocalQuit));
        }

        [Test]
        public void IgnoresPingsSoAnRttProbeCanShareTheLink()
        {
            var match = new Match(16, autoPlay: false).Run();
            match.Hub.B.Send(new PingMessage(false, 1).Encode());
            match.Hub.DeliverAll();
            Assert.That(match.Host.Phase, Is.EqualTo(SessionPhase.Playing));
        }

        /// <summary>Starts a host, has a raw peer send <paramref name="message"/>, and returns why the host aborted.</summary>
        static AbortReason PeerSays(byte[] message)
        {
            var hub = new LoopbackHub();
            var host = new ShootoutSession(DuelRole.Host, hub.A, new ShootoutSessionOptions { Nonces = new InsecureSeededNonceSource(1) });
            hub.Connect();
            host.Start();
            hub.B.Send(message);
            hub.DeliverAll();
            return host.AbortReason;
        }
    }
}
