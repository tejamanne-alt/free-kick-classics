using System;
using System.Collections.Generic;

namespace FreeKickClassics.DuelLink
{
    /// <summary>
    /// Two in-memory <see cref="IDuelLink"/> ends joined by one ordered queue. Nothing is delivered until
    /// <see cref="DeliverNext"/> or <see cref="DeliverAll"/> is called, so tests control interleaving exactly and
    /// handlers never re-enter each other. Also useful for a same-device "hot seat" debug build.
    /// </summary>
    public sealed class LoopbackHub
    {
        readonly Queue<Envelope> queue = new Queue<Envelope>();

        public LoopbackHub(int maxMessageSize = 20)
        {
            if (maxMessageSize < 1) throw new ArgumentOutOfRangeException(nameof(maxMessageSize));
            A = new LoopbackLink(this, maxMessageSize);
            B = new LoopbackLink(this, maxMessageSize);
        }

        public LoopbackLink A { get; }

        public LoopbackLink B { get; }

        /// <summary>
        /// Optional hook run on every message as it is sent. Return the bytes to deliver (the same array, a
        /// tampered copy) or null to drop the message. Lets tests play a man in the middle or a hacked phone.
        /// </summary>
        public Func<LoopbackLink, byte[], byte[]> Intercept { get; set; }

        public int Pending => queue.Count;

        /// <summary>Every message handed to <see cref="IDuelLink.Send"/> on either end, in order.</summary>
        public List<(LoopbackLink From, byte[] Data)> SentLog { get; } = new List<(LoopbackLink, byte[])>();

        public void Connect()
        {
            A.SetState(LinkState.Connected);
            B.SetState(LinkState.Connected);
        }

        /// <summary>Drops the connection and every message still in flight, like walking out of Bluetooth range.</summary>
        public void Disconnect()
        {
            queue.Clear();
            A.SetState(LinkState.Disconnected);
            B.SetState(LinkState.Disconnected);
        }

        public bool DeliverNext()
        {
            if (queue.Count == 0) return false;
            var envelope = queue.Dequeue();
            envelope.To.Deliver(envelope.Data);
            return true;
        }

        /// <returns>How many messages were delivered.</returns>
        public int DeliverAll(int maxMessages = 100_000)
        {
            int delivered = 0;
            while (delivered < maxMessages && DeliverNext()) delivered++;
            if (queue.Count > 0) throw new InvalidOperationException("Message storm: more than " + maxMessages + " deliveries.");
            return delivered;
        }

        internal void Enqueue(LoopbackLink from, byte[] data)
        {
            SentLog.Add((from, (byte[])data.Clone()));
            var outgoing = Intercept == null ? (byte[])data.Clone() : Intercept(from, (byte[])data.Clone());
            if (outgoing == null) return;
            queue.Enqueue(new Envelope(from == A ? B : A, outgoing));
        }

        readonly struct Envelope
        {
            public readonly LoopbackLink To;
            public readonly byte[] Data;

            public Envelope(LoopbackLink to, byte[] data)
            {
                To = to;
                Data = data;
            }
        }
    }

    public sealed class LoopbackLink : IDuelLink
    {
        readonly LoopbackHub hub;

        internal LoopbackLink(LoopbackHub hub, int maxMessageSize)
        {
            this.hub = hub;
            MaxMessageSize = maxMessageSize;
        }

        public LinkState State { get; private set; } = LinkState.Disconnected;

        public int MaxMessageSize { get; }

        /// <summary>Messages dropped because they were sent while disconnected.</summary>
        public int DroppedWhileDisconnected { get; private set; }

        public event Action<LinkState> StateChanged;

        public event Action<byte[]> MessageReceived;

        public void Send(byte[] message)
        {
            if (message == null || message.Length == 0) throw new ArgumentException("Message is empty.", nameof(message));
            if (message.Length > MaxMessageSize)
            {
                throw new ArgumentException(
                    "Message is " + message.Length + " bytes; this link carries at most " + MaxMessageSize + ".",
                    nameof(message));
            }

            if (State != LinkState.Connected)
            {
                DroppedWhileDisconnected++;
                return;
            }

            hub.Enqueue(this, message);
        }

        internal void SetState(LinkState state)
        {
            if (State == state) return;
            State = state;
            StateChanged?.Invoke(state);
        }

        internal void Deliver(byte[] data)
        {
            if (State != LinkState.Connected) return;
            MessageReceived?.Invoke(data);
        }
    }
}
