using System;

namespace FreeKickClassics.DuelLink
{
    /// <summary>A message could not be decoded: wrong length, wrong type byte or an out-of-range field.</summary>
    public sealed class ProtocolException : Exception
    {
        public ProtocolException(string message) : base(message)
        {
        }
    }
}
