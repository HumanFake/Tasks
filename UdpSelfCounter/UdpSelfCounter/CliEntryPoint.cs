using System;
using NetUtils;

namespace UdpSelfCounter
{
    internal static class CliEntryPoint
    {
        private static void Main()
        {
            var localPort = Port.ReadPort();
            try
            {
                using (var sender = new Sender(localPort))
                {
                    sender.Send(ProgramData.EntryMessage);

                    using (var receiver = new Receiver(sender, localPort))
                    {
                        receiver.Listen();
                    }
                }
            }
            catch (ReceiveException)
            {
                Console.WriteLine("error when message is received");
            }
            catch (SenderException)
            {
                Console.WriteLine("error when message is sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
