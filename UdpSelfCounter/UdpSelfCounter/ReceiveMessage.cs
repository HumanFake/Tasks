using System;
using System.Net;
using JetBrains.Annotations;

namespace UdpSelfCounter
{
    internal sealed class ReceiveMessage
    {
        internal ReceiveMessage([NotNull] byte[] message, [NotNull] IPEndPoint endPoint)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            Address = endPoint.Address;
            try
            {
                var returnData = BitConverter.ToInt16(message, 0);
                Body = returnData;
            }
            catch (Exception)
            {
                Body = ProgramData.InvalidMessage;
            }
        }
        
        internal short Body { get; }
        internal IPAddress Address { get; }
    }
}