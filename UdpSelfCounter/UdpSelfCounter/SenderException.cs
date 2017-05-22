using System;

namespace UdpSelfCounter
{
    internal sealed class SenderException : Exception
    {
        public SenderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}