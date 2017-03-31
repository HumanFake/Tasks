using System.Net;

namespace UdpSelfCounter
{
    internal static class ProgramData
    {
        internal const short InvalidMessage = 0;
        internal const short EntryMessage = 1;
        internal const short OutMessage = 2;
        internal const short AnswerMessage = 3;

        internal static readonly IPAddress BroadcasAddress = IPAddress.Parse("239.255.255.1");
    }
}