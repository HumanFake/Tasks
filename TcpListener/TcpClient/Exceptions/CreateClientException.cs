using System;

namespace TcpClient.Exceptions
{
    public sealed class CreateClientException : Exception
    {
        public CreateClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}