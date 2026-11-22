using System;
using System.Collections.Generic;
using FreeKickClassics.Core.Physics;
using FreeKickClassics.DuelLink;
using FreeKickClassics.DuelLink.Crypto;
using FreeKickClassics.DuelLink.Wire;

namespace FreeKickClassics.Core.Shootout
{
    public enum SessionPhase
    {
        Idle,
        Handshaking,
        Playing,
        Finished,
        Aborted,
    }

    /// <summary>Why a match ended early. Sent to the other phone in an Abort message.</summary>
    public enum AbortReason : byte
    {
        None = 0,
        ProtocolVersionMismatch = 1,
        WrongGame = 2,
        RoleConflict = 3,
        RulesMismatch = 4,
        MalformedMessage = 5,
        UnexpectedMessage = 6,
        /// <summary>The other phone revealed a pick that doesn't match its commitment: a bug or a hacked phone.</summary>
        CommitmentMismatch = 7,
        InvalidChoice = 8,
        /// <summary>The host reported a result the guest's own resolution disagrees with.</summary>
        OutcomeMismatch = 9,
        RemoteAborted = 10,
        LocalQuit = 11,
    }

    public sealed class RoundReport
    {
        internal RoundReport(int round, DuelRole kicker, KickerChoice kick, KeeperChoice save, ShotResolution resolution, LaunchParams launch, Vec3 endPoint, double flightSeconds, int hostGoals, int guestGoals)
        {
            Round = round;
            Kicker = kicker;
            Kick = kick;
            Save = save;
            Resolution = resolution;
            Launch = launch;
            EndPoint = endPoint;
            FlightSeconds = flightSeconds;
            HostGoals = hostGoals;
            GuestGoals = guestGoals;
        }

        public int Round { get; }

        public DuelRole Kicker { get; }

        public DuelRole Keeper => Kicker.Other();

        public KickerChoice Kick { get; }

        public KeeperChoice Save { get; }

        public ShotResolution Resolution { get; }

        public ShotOutcome Outcome => Resolution.Outcome;

        /// <summary>Replay this through <see cref="ShootoutSession.FlightModel"/>.</summary>
        public LaunchParams Launch { get; }

        /// <summary>The host's end point. On the guest, steer the replay onto it with <see cref="TrajectoryReconciler"/>.</summary>
        public Vec3 EndPoint { get; }

        public double FlightSeconds { get; }

        public int HostGoals { get; }

        public int GuestGoals { get; }
    }

    public sealed class ShootoutSessionOptions
    {
        /// <summary>Host only: era, weather and length. Defaults to <see cref="MatchSetup.Standard"/>.</summary>
        public MatchSetup Setup { get; set; }

        public ShootoutTuning Tuning { get; set; }

        /// <summary>Defaults to the OS secure random generator.</summary>
        public INonceSource Nonces { get; set; }

        /// <summary>Host only. Defaults to <see cref="PenaltyKickPlanner"/>.</summary>
        public Func<FlightModel, IKickPlanner> PlannerFactory { get; set; }
    }

    /// <summary>
    /// One Stare-Down Shootout between two phones over an <see cref="IDuelLink"/>.
    /// </summary>
    /// <remarks>
    /// <para>Each kick: both phones send a commitment to their secret pick; once a phone holds the other's commitment it
    /// reveals its pick and nonce; once both picks are verified, both phones resolve the kick with the same shared seed.
    /// The host plans the flight and sends Launch and Result; the guest checks the result against its own resolution.</para>
    /// <para>Every outgoing message is kept per round. When the link reconnects, the session resends the handshake and
    /// the last two rounds' messages; receivers ignore exact duplicates and anything from a round they've finished.
    /// That is the whole reconnect story: no extra resync messages.</para>
    /// <para>Single-threaded: call it, and raise the link's events, from the game's main thread.</para>
    /// </remarks>
    public sealed class ShootoutSession : IDisposable
    {
        public const byte ProtocolVersion = 1;
        public const byte GameId = 0x01;

        /// <summary>How far the host's end point may be from the agreed target before the guest calls foul.</summary>
        public const double EndPointToleranceMetres = 0.10;

        const ulong KnuckleSalt = 0x6B6E75636B6C6531UL;

        readonly IDuelLink link;
        readonly ShootoutTuning tuning;
        readonly PenaltyResolver resolver;
        readonly INonceSource nonces;
        readonly Func<FlightModel, IKickPlanner> plannerFactory;
        readonly MatchSetup hostSetup;
        readonly List<KickerChoice>[] kicksBy = { new List<KickerChoice>(), new List<KickerChoice>() };
        readonly List<KeeperChoice>[] savesBy = { new List<KeeperChoice>(), new List<KeeperChoice>() };

        byte[] localHelloNonce;
        byte[] localHelloMessage;
        byte[] remoteHelloNonce;
        byte[] setupMessage;
        MatchSetup pendingSetup;
        IKickPlanner planner;
        Round current;
        Round previous;
        DuelRole firstKicker;

        public ShootoutSession(DuelRole role, IDuelLink link, ShootoutSessionOptions options = null)
        {
            options ??= new ShootoutSessionOptions();
            Role = role;
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            tuning = options.Tuning ?? ShootoutTuning.CreateDefault();
            resolver = new PenaltyResolver(tuning);
            nonces = options.Nonces ?? new SystemNonceSource();
            plannerFactory = options.PlannerFactory ?? (model => new PenaltyKickPlanner(model));

            if (role == DuelRole.Host)
            {
                hostSetup = options.Setup ?? MatchSetup.Standard(tuning);
                if (hostSetup.TuningFingerprint != tuning.Fingerprint())
                {
                    throw new ArgumentException("The match setup's tuning fingerprint doesn't match this session's tuning.", nameof(options));
                }
            }

            if (link.MaxMessageSize < 18) throw new ArgumentException("The link must carry at least 18-byte messages.", nameof(link));
            link.MessageReceived += OnMessage;
            link.StateChanged += OnLinkStateChanged;
        }

        public event Action<SessionPhase> PhaseChanged;

        /// <summary>A new kick is ready to be picked. Arguments: round index, who kicks.</summary>
        public event Action<int, DuelRole> RoundStarted;

        /// <summary>The other player has locked in a pick for this round (their pick is still secret).</summary>
        public event Action<int> RemoteLockedIn;

        public event Action<RoundReport> RoundCompleted;

        public event Action<AbortReason> Aborted;

        public DuelRole Role { get; }

        public SessionPhase Phase { get; private set; } = SessionPhase.Idle;

        public AbortReason AbortReason { get; private set; }

        /// <summary>The reason the other phone gave, when <see cref="AbortReason"/> is <see cref="Shootout.AbortReason.RemoteAborted"/>.</summary>
        public AbortReason RemoteAbortReason { get; private set; }

        public byte[] SessionId { get; private set; }

        /// <summary>Known once the host's setup is in.</summary>
        public MatchSetup Setup { get; private set; }

        /// <summary>The flight model for replays, built from <see cref="Setup"/>.</summary>
        public FlightModel FlightModel { get; private set; }

        public ShootoutScore Score { get; private set; }

        public PenaltyResolver Resolver => resolver;

        public int CurrentRound => current?.Index ?? -1;

        public DuelRole CurrentKicker => current?.Kicker ?? DuelRole.Host;

        public bool IsLocalKicker => current != null && current.Kicker == Role;

        public bool HasLocallyCommitted => current?.LocalCommitment != null;

        public bool HasRemoteCommitted => current?.RemoteCommitment != null;

        public IReadOnlyList<KickerChoice> KicksBy(DuelRole role) => kicksBy[(int)role];

        public IReadOnlyList<KeeperChoice> SavesBy(DuelRole role) => savesBy[(int)role];

        /// <summary>Sends Hello. Safe to call before the link connects; it is resent once it does.</summary>
        public void Start()
        {
            if (Phase != SessionPhase.Idle) throw new InvalidOperationException("Already started.");
            localHelloNonce = new byte[HelloMessage.NonceSize];
            nonces.Fill(localHelloNonce);
            localHelloMessage = new HelloMessage(ProtocolVersion, GameId, Role, localHelloNonce).Encode();
            SetPhase(SessionPhase.Handshaking);
            link.Send(localHelloMessage);
            TryCompleteHandshake();
        }

        public void SubmitKick(KickerChoice choice)
        {
            RequireTurn(isKicker: true);
            Commit(choice.Encode());
        }

        public void SubmitSave(KeeperChoice choice)
        {
            RequireTurn(isKicker: false);
            Commit(choice.Encode());
        }

        public void Quit() => Abort(AbortReason.LocalQuit);

        public void Dispose()
        {
            link.MessageReceived -= OnMessage;
            link.StateChanged -= OnLinkStateChanged;
        }

        void RequireTurn(bool isKicker)
        {
            if (Phase != SessionPhase.Playing) throw new InvalidOperationException("No kick in progress (phase " + Phase + ").");
            if (IsLocalKicker != isKicker) throw new InvalidOperationException(isKicker ? "This phone is keeping this round." : "This phone is kicking this round.");
            if (current.LocalCommitment != null) throw new InvalidOperationException("Already locked in for this round.");
        }

        void Commit(byte[] payload)
        {
            var round = current;
            round.LocalPayload = payload;
            round.LocalNonce = new byte[Commitment.NonceSize];
            nonces.Fill(round.LocalNonce);
            round.LocalCommitment = Commitment.Compute(SessionId, round.Index, Role, payload, round.LocalNonce);
            if (round.Kicker == Role) KickerChoice.TryDecode(payload, out round.Kick);
            else KeeperChoice.TryDecode(payload, out round.Save);

            Send(round, new CommitMessage((byte)round.Index, round.LocalCommitment).Encode());
            Advance();
        }

        void OnLinkStateChanged(LinkState state)
        {
            if (state != LinkState.Connected || Phase == SessionPhase.Idle || Phase == SessionPhase.Aborted) return;

            link.Send(localHelloMessage);
            if (setupMessage != null && Role == DuelRole.Host) link.Send(setupMessage);
            if (previous != null)
            {
                foreach (var message in previous.Outbox) link.Send(message);
            }

            if (current != null)
            {
                foreach (var message in current.Outbox) link.Send(message);
            }
        }

        void OnMessage(byte[] data)
        {
            if (Phase == SessionPhase.Aborted) return;
            if (data == null || data.Length == 0)
            {
                Abort(AbortReason.MalformedMessage);
                return;
            }

            try
            {
                switch (data[0])
                {
                    case LinkMessageType.Hello: HandleHello(HelloMessage.Decode(data)); break;
                    case ShootoutMessageType.MatchSetup: HandleSetup(MatchSetup.Decode(data)); break;
                    case LinkMessageType.Commit: HandleCommit(CommitMessage.Decode(data)); break;
                    case LinkMessageType.Reveal: HandleReveal(RevealMessage.Decode(data)); break;
                    case ShootoutMessageType.Launch: HandleLaunch(LaunchMessage.Decode(data)); break;
                    case ShootoutMessageType.Result: HandleResult(ResultMessage.Decode(data)); break;
                    case LinkMessageType.Abort: HandleRemoteAbort(AbortMessage.Decode(data)); break;
                    case LinkMessageType.Ping:
                    case LinkMessageType.Pong:
                        break; // Handled by RttProbe.
                    default:
                        Abort(AbortReason.MalformedMessage);
                        break;
                }
            }
            catch (ProtocolException)
            {
                Abort(AbortReason.MalformedMessage);
            }
        }

        void HandleHello(HelloMessage hello)
        {
            if (hello.ProtocolVersion != ProtocolVersion)
            {
                Abort(AbortReason.ProtocolVersionMismatch);
                return;
            }

            if (hello.GameId != GameId)
            {
                Abort(AbortReason.WrongGame);
                return;
            }

            if (hello.Role == Role)
            {
                Abort(AbortReason.RoleConflict);
                return;
            }

            if (remoteHelloNonce != null)
            {
                // A replay after a reconnect is fine; a new nonce means the other app restarted and lost the match.
                if (!SameBytes(remoteHelloNonce, hello.Nonce)) Abort(AbortReason.UnexpectedMessage);
                return;
            }

            remoteHelloNonce = hello.Nonce;
            TryCompleteHandshake();
        }

        void TryCompleteHandshake()
        {
            if (Phase != SessionPhase.Handshaking || SessionId != null || localHelloNonce == null || remoteHelloNonce == null) return;

            byte[] hostNonce = Role == DuelRole.Host ? localHelloNonce : remoteHelloNonce;
            byte[] guestNonce = Role == DuelRole.Host ? remoteHelloNonce : localHelloNonce;
            SessionId = SharedRandomness.SessionId(hostNonce, guestNonce);
            firstKicker = SharedRandomness.CoinToss(SessionId) ? DuelRole.Host : DuelRole.Guest;

            if (Role == DuelRole.Host)
            {
                setupMessage = hostSetup.Encode();
                link.Send(setupMessage);
                ApplySetup(MatchSetup.Decode(setupMessage));
            }
            else if (pendingSetup != null)
            {
                var setup = pendingSetup;
                pendingSetup = null;
                ApplySetup(setup);
            }
        }

        void HandleSetup(MatchSetup setup)
        {
            if (Role == DuelRole.Host)
            {
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (Setup != null || pendingSetup != null)
            {
                if (!setup.Equals(Setup ?? pendingSetup)) Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (SessionId == null)
            {
                pendingSetup = setup; // Arrived before this phone called Start().
                return;
            }

            ApplySetup(setup);
        }

        void ApplySetup(MatchSetup setup)
        {
            if (setup.TuningFingerprint != tuning.Fingerprint())
            {
                Abort(AbortReason.RulesMismatch);
                return;
            }

            Setup = setup;
            FlightModel = new FlightModel(EraProfile.For(setup.Era), setup.Conditions);
            if (Role == DuelRole.Host) planner = plannerFactory(FlightModel);
            Score = new ShootoutScore(firstKicker, setup.KicksPerSide);
            SetPhase(SessionPhase.Playing);
            StartRound(0);
        }

        void StartRound(int index)
        {
            previous = current;
            current = new Round(index, Score.NextKicker);
            RoundStarted?.Invoke(index, current.Kicker);
        }

        /// <summary>Places a message's round: true to process it, false to ignore it as a stale duplicate.</summary>
        bool AcceptRound(byte round)
        {
            if (Phase != SessionPhase.Playing && Phase != SessionPhase.Finished)
            {
                Abort(AbortReason.UnexpectedMessage);
                return false;
            }

            int delta = WireChecks.RoundDelta(round, current.Index);
            if (delta > 0)
            {
                Abort(AbortReason.UnexpectedMessage);
                return false;
            }

            return delta == 0 && !current.Complete;
        }

        void HandleCommit(CommitMessage message)
        {
            if (!AcceptRound(message.Round)) return;
            var round = current;
            if (round.RemoteCommitment != null)
            {
                if (!SameBytes(round.RemoteCommitment, message.Commitment)) Abort(AbortReason.UnexpectedMessage);
                return;
            }

            round.RemoteCommitment = message.Commitment;
            RemoteLockedIn?.Invoke(round.Index);
            Advance();
        }

        void HandleReveal(RevealMessage message)
        {
            if (!AcceptRound(message.Round)) return;
            var round = current;
            if (round.RemoteCommitment == null || round.LocalCommitment == null)
            {
                // An honest phone only reveals after it has seen our commitment, which comes after its own.
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (round.RemotePayload != null)
            {
                if (!SameBytes(round.RemotePayload, message.Payload) || !SameBytes(round.RemoteNonce, message.Nonce)) Abort(AbortReason.UnexpectedMessage);
                return;
            }

            DuelRole remote = Role.Other();
            if (!Commitment.Verify(round.RemoteCommitment, SessionId, round.Index, remote, message.Payload, message.Nonce))
            {
                Abort(AbortReason.CommitmentMismatch);
                return;
            }

            bool decoded = round.Kicker == remote
                ? KickerChoice.TryDecode(message.Payload, out round.Kick)
                : KeeperChoice.TryDecode(message.Payload, out round.Save);
            if (!decoded)
            {
                Abort(AbortReason.InvalidChoice);
                return;
            }

            round.RemotePayload = message.Payload;
            round.RemoteNonce = message.Nonce;
            Advance();
        }

        void HandleLaunch(LaunchMessage message)
        {
            if (Role == DuelRole.Host)
            {
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (!AcceptRound(message.Round)) return;
            if (current.Launch.HasValue)
            {
                // A replay after a reconnect that dropped the Result.
                if (!SameBytes(current.Launch.Value.Encode(), message.Encode())) Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (!current.Resolved)
            {
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            current.Launch = message;
            Advance();
        }

        void HandleResult(ResultMessage message)
        {
            if (Role == DuelRole.Host)
            {
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            if (!AcceptRound(message.Round)) return;
            if (!current.Launch.HasValue)
            {
                Abort(AbortReason.UnexpectedMessage);
                return;
            }

            current.Result = message;
            Advance();
        }

        void HandleRemoteAbort(AbortMessage message)
        {
            RemoteAbortReason = (AbortReason)message.Reason;
            Abort(AbortReason.RemoteAborted);
        }

        void Advance()
        {
            var round = current;
            if (round.Complete || Phase != SessionPhase.Playing) return;

            if (round.LocalCommitment != null && round.RemoteCommitment != null && !round.RevealSent)
            {
                round.RevealSent = true;
                Send(round, new RevealMessage((byte)round.Index, round.LocalPayload, round.LocalNonce).Encode());
            }

            if (round.LocalPayload == null || round.RemotePayload == null) return;

            if (!round.Resolved)
            {
                byte[] hostNonce = Role == DuelRole.Host ? round.LocalNonce : round.RemoteNonce;
                byte[] guestNonce = Role == DuelRole.Host ? round.RemoteNonce : round.LocalNonce;
                round.Seed = SharedRandomness.RoundSeed(SessionId, round.Index, hostNonce, guestNonce);
                round.Resolution = resolver.Resolve(round.Kick, round.Save, round.Seed);
                round.Resolved = true;

                if (Role == DuelRole.Host)
                {
                    var planned = planner.Plan(round.Kick, round.Resolution, round.Seed ^ KnuckleSalt);
                    Send(round, new LaunchMessage((byte)round.Index, planned.Launch).Encode());
                    Send(round, new ResultMessage((byte)round.Index, round.Resolution.Outcome, planned.EndPoint, planned.FlightSeconds).Encode());
                    CompleteRound(round, planned.Launch, planned.EndPoint, planned.FlightSeconds);
                    return;
                }
            }

            if (Role == DuelRole.Guest && round.Launch.HasValue && round.Result.HasValue)
            {
                var result = round.Result.Value;
                Vec3 end = result.EndPoint;
                bool sameOutcome = result.Outcome == round.Resolution.Outcome;
                bool closeToTarget =
                    System.Math.Abs(end.X - round.Resolution.TargetXmm / 1000.0) <= EndPointToleranceMetres &&
                    System.Math.Abs(end.Y - round.Resolution.TargetYmm / 1000.0) <= EndPointToleranceMetres;
                if (!sameOutcome || !closeToTarget)
                {
                    Abort(AbortReason.OutcomeMismatch);
                    return;
                }

                var launch = round.Launch.Value.ToLaunch(Pitch.PenaltySpot, round.Seed ^ KnuckleSalt);
                CompleteRound(round, launch, end, result.FlightSeconds);
            }
        }

        void CompleteRound(Round round, LaunchParams launch, Vec3 endPoint, double flightSeconds)
        {
            round.Complete = true;
            DuelRole keeper = round.Kicker.Other();
            kicksBy[(int)round.Kicker].Add(round.Kick);
            savesBy[(int)keeper].Add(round.Save);
            Score.Record(round.Kicker, round.Resolution.IsGoal);

            var report = new RoundReport(
                round.Index, round.Kicker, round.Kick, round.Save, round.Resolution, launch, endPoint, flightSeconds,
                Score.Goals(DuelRole.Host), Score.Goals(DuelRole.Guest));
            RoundCompleted?.Invoke(report);
            if (Phase != SessionPhase.Playing) return; // A handler quit.

            if (Score.IsDecided)
            {
                SetPhase(SessionPhase.Finished);
                return;
            }

            StartRound(round.Index + 1);
        }

        void Send(Round round, byte[] message)
        {
            round.Outbox.Add(message);
            link.Send(message);
        }

        void Abort(AbortReason reason)
        {
            if (Phase == SessionPhase.Aborted) return;
            AbortReason = reason;
            if (reason != AbortReason.RemoteAborted) link.Send(new AbortMessage((byte)reason).Encode());
            SetPhase(SessionPhase.Aborted);
            Aborted?.Invoke(reason);
        }

        void SetPhase(SessionPhase phase)
        {
            if (Phase == phase) return;
            Phase = phase;
            PhaseChanged?.Invoke(phase);
        }

        static bool SameBytes(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        sealed class Round
        {
            public readonly int Index;
            public readonly DuelRole Kicker;
            public readonly List<byte[]> Outbox = new List<byte[]>();

            public byte[] LocalPayload;
            public byte[] LocalNonce;
            public byte[] LocalCommitment;
            public byte[] RemoteCommitment;
            public byte[] RemotePayload;
            public byte[] RemoteNonce;
            public bool RevealSent;

            public KickerChoice Kick;
            public KeeperChoice Save;
            public bool Resolved;
            public ulong Seed;
            public ShotResolution Resolution;
            public LaunchMessage? Launch;
            public ResultMessage? Result;
            public bool Complete;

            public Round(int index, DuelRole kicker)
            {
                Index = index;
                Kicker = kicker;
            }
        }
    }
}
