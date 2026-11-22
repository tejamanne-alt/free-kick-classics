using System;

namespace FreeKickClassics.DuelLink
{
    /// <summary>Which phone a player is on. The host is the BLE peripheral and runs the physics.</summary>
    public enum DuelRole : byte
    {
        Host = 0,
        Guest = 1,
    }

    public static class DuelRoleExtensions
    {
        public static DuelRole Other(this DuelRole role) => role == DuelRole.Host ? DuelRole.Guest : DuelRole.Host;
    }

    public enum LinkState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    /// <summary>
    /// An established, ordered, reliable message link to the other phone. Bluetooth LE, same-Wi-Fi UDP and
    /// online play all sit behind this one interface.
    /// </summary>
    /// <remarks>
    /// Contract for implementations:
    /// <list type="bullet">
    /// <item>Messages arrive whole, in the order they were sent, while the link stays connected.</item>
    /// <item><see cref="Send"/> while not <see cref="LinkState.Connected"/> drops the message silently. Callers keep
    /// their own outbox and replay it when the link reconnects (see <c>ShootoutSession</c>).</item>
    /// <item>Events are raised on the game's main thread. Native callbacks must be queued and pumped from Unity's
    /// update loop, never raised directly from a Bluetooth thread.</item>
    /// <item><see cref="MessageReceived"/> hands over a fresh array the receiver may keep.</item>
    /// </list>
    /// </remarks>
    public interface IDuelLink
    {
        LinkState State { get; }

        /// <summary>Largest message one <see cref="Send"/> may carry. 20 bytes on a default BLE ATT MTU of 23.</summary>
        int MaxMessageSize { get; }

        event Action<LinkState> StateChanged;

        event Action<byte[]> MessageReceived;

        /// <exception cref="ArgumentException">The message is empty or longer than <see cref="MaxMessageSize"/>.</exception>
        void Send(byte[] message);
    }
}
