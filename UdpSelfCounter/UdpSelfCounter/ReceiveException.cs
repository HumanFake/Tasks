using System;

namespace UdpSelfCounter
{
    internal sealed class ReceiveException : Exception
    {
        internal ReceiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}