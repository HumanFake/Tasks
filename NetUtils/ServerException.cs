using System;

namespace NetUtils
{
    public class ServerException : Exception
    {
        private const string DefaultMessage = "Ошибка в работе сервера";

        public ServerException(Exception innerException, string message = DefaultMessage) : base(message, innerException)
        {
        }
    }
}