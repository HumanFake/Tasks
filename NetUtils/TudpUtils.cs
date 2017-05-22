using IdentifierType = System.Int32;

namespace NetUtils
{
    public static class TudpUtils
    {
        public const int MaxDatagramByteCount = 60000;
        public const int IdentifierByteCount = sizeof(IdentifierType);
    }
}