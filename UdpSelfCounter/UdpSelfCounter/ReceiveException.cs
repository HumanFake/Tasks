using System;

namespace UdpSelfCounter
{
    internal sealed class ReceiveException : Exception
    {
        public ReceiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}