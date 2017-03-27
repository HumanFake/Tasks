using System;

namespace TcpClient.Exceptions
{
    public class SendDataException : Exception
    {
        public SendDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}