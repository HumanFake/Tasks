using System;

namespace NetUtils
{
    public sealed class ServerException : Exception
    {
        private const string DefaultMessage = "server error";

        public ServerException(Exception innerException, string message = DefaultMessage) : base(message, innerException)
        {
        }
    }
}