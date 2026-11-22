using System;
using System.Globalization;

namespace FreeKickClassics.DuelLink.Discovery
{
    /// <summary>
    /// Builds and reads the 128-bit service UUID a host advertises over BLE. The room code lives inside the UUID
    /// because a service UUID is the only advertising field both platforms let an app set as a peripheral: iOS only
    /// advertises a local name and service UUIDs, and Android can only advertise the phone's own Bluetooth name,
    /// not a custom one.
    /// </summary>
    /// <remarks>
    /// Layout: <c>c6eb9b26-a6da-4739-889b-6fa4GGVVRRRR</c>. The first 96 bits are fixed (Android guests can scan with
    /// a UUID mask on them), then one byte of game id, one byte of protocol version and a 16-bit room code. An iOS
    /// guest that has the code typed in builds the exact UUID and scans for it.
    /// </remarks>
    public static class RoomAdvertisement
    {
        public const string BasePrefix = "c6eb9b26-a6da-4739-889b-6fa4";

        /// <summary>Guest writes host-bound messages here (write with response).</summary>
        public const string ToHostCharacteristic = "fb4c6981-1f5e-4f3a-9b3c-21272af45b9b";

        /// <summary>Host sends guest-bound messages here (notify).</summary>
        public const string ToGuestCharacteristic = "a3cc2bbd-983a-428e-8a6e-2bebbb302a90";

        public const int MaxRoomCode = 9999;

        public static string ServiceUuid(byte gameId, byte protocolVersion, int roomCode)
        {
            if (roomCode < 0 || roomCode > MaxRoomCode) throw new ArgumentOutOfRangeException(nameof(roomCode), "Room codes are 0000-9999.");
            return BasePrefix + gameId.ToString("x2") + protocolVersion.ToString("x2") + roomCode.ToString("x4");
        }

        public static bool TryParse(string uuid, out byte gameId, out byte protocolVersion, out int roomCode)
        {
            gameId = 0;
            protocolVersion = 0;
            roomCode = 0;
            if (uuid == null || uuid.Length != 36) return false;
            if (!uuid.StartsWith(BasePrefix, StringComparison.OrdinalIgnoreCase)) return false;

            string tail = uuid.Substring(BasePrefix.Length);
            if (!byte.TryParse(tail.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out gameId)) return false;
            if (!byte.TryParse(tail.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out protocolVersion)) return false;
            if (!int.TryParse(tail.Substring(4, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out roomCode)) return false;
            return roomCode <= MaxRoomCode;
        }

        /// <summary>A room code to show on the host's screen, e.g. "0427".</summary>
        public static string FormatRoomCode(int roomCode) => roomCode.ToString("D4", CultureInfo.InvariantCulture);
    }
}
