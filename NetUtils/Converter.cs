using JetBrains.Annotations;

namespace NetUtils
{
    public static class Converter
    {
        private const double BytesInMegaBytes = 1024.0;
        private const double MillisecondsInSecond = 1000.0;

        [PublicAPI]
        public static double BytesToMegaBytes(this long source)
        {
            return source / BytesInMegaBytes / BytesInMegaBytes;
        }

        [PublicAPI]
        public static double MillisecondToSecond(this long source)
        {
            return source / MillisecondsInSecond;
        }
    }
}
