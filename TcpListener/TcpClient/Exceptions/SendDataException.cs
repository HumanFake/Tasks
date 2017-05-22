using System;

namespace TcpClient.Exceptions
{
    public sealed class SendDataException : Exception
    {
        public SendDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}