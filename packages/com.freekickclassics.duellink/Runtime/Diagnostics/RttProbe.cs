using System;
using System.Collections.Generic;
using FreeKickClassics.DuelLink.Wire;

namespace FreeKickClassics.DuelLink.Diagnostics
{
    /// <summary>
    /// Measures round-trip time over a link with Ping/Pong messages, for the Bluetooth spike's phone matrix.
    /// Attach one to each phone's link; by default each probe also answers the other's pings.
    /// </summary>
    public sealed class RttProbe : IDisposable
    {
        readonly IDuelLink link;
        readonly Func<double> clockSeconds;
        readonly bool answerPings;
        readonly Dictionary<ushort, double> inFlight = new Dictionary<ushort, double>();
        readonly List<double> samples = new List<double>();
        ushort nextSequence;

        /// <param name="clockSeconds">Monotonic clock, e.g. <c>() => Time.realtimeSinceStartupAsDouble</c>.</param>
        public RttProbe(IDuelLink link, Func<double> clockSeconds, bool answerPings = true)
        {
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            this.clockSeconds = clockSeconds ?? throw new ArgumentNullException(nameof(clockSeconds));
            this.answerPings = answerPings;
            link.MessageReceived += OnMessage;
        }

        public int Sent { get; private set; }

        public IReadOnlyList<double> SamplesSeconds => samples;

        public void SendPing()
        {
            ushort sequence = nextSequence++;
            inFlight[sequence] = clockSeconds();
            Sent++;
            link.Send(new PingMessage(false, sequence).Encode());
        }

        public RttStats GetStats() => RttStats.From(samples, Sent);

        public void Dispose() => link.MessageReceived -= OnMessage;

        void OnMessage(byte[] data)
        {
            if (data.Length == 0 || (data[0] != LinkMessageType.Ping && data[0] != LinkMessageType.Pong)) return;

            PingMessage message;
            try
            {
                message = PingMessage.Decode(data);
            }
            catch (ProtocolException)
            {
                return;
            }

            if (!message.IsPong)
            {
                if (answerPings) link.Send(new PingMessage(true, message.Sequence).Encode());
                return;
            }

            if (inFlight.TryGetValue(message.Sequence, out double sentAt))
            {
                inFlight.Remove(message.Sequence);
                samples.Add(clockSeconds() - sentAt);
            }
        }
    }

    public readonly struct RttStats
    {
        public readonly int Count;
        public readonly int Lost;
        public readonly double Min;
        public readonly double Median;
        public readonly double P95;
        public readonly double Max;
        public readonly double Mean;

        RttStats(int count, int lost, double min, double median, double p95, double max, double mean)
        {
            Count = count;
            Lost = lost;
            Min = min;
            Median = median;
            P95 = p95;
            Max = max;
            Mean = mean;
        }

        /// <param name="sent">Pings sent; anything unanswered counts as lost.</param>
        public static RttStats From(IReadOnlyList<double> samplesSeconds, int sent)
        {
            int count = samplesSeconds.Count;
            int lost = Math.Max(0, sent - count);
            if (count == 0) return new RttStats(0, lost, 0, 0, 0, 0, 0);

            var sorted = new double[count];
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                sorted[i] = samplesSeconds[i];
                sum += sorted[i];
            }

            Array.Sort(sorted);
            return new RttStats(count, lost, sorted[0], Percentile(sorted, 0.5), Percentile(sorted, 0.95), sorted[count - 1], sum / count);
        }

        /// <summary>Nearest-rank percentile.</summary>
        static double Percentile(double[] sorted, double p)
        {
            int rank = (int)Math.Ceiling(p * sorted.Length);
            return sorted[Math.Max(0, Math.Min(sorted.Length - 1, rank - 1))];
        }

        public override string ToString() =>
            string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "n={0} lost={1} min={2:F1}ms median={3:F1}ms p95={4:F1}ms max={5:F1}ms",
                Count, Lost, Min * 1000, Median * 1000, P95 * 1000, Max * 1000);
    }
}
