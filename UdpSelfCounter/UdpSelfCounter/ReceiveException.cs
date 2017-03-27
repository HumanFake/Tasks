using System;

namespace UdpSelfCounter
{
    internal class ReceiveException : Exception
    {
        public ReceiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}