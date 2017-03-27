namespace NetUtils
{
    public sealed class Port
    {
        public Port(int port)
        {
            GetPort = port;
        }

        public int GetPort { get; private set; }
    }
}