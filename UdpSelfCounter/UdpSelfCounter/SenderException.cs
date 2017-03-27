using System;

namespace UdpSelfCounter
{
    internal class SenderException : Exception
    {
        public SenderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}