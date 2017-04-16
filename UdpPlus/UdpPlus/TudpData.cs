using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    internal sealed class TudpData
    {
        private readonly byte[] _identifer;
        private readonly List<byte> _data = new List<byte>();

        internal const int LastMessegeIndetifer = -1;
        internal readonly static byte[] LastMessege = BitConverter.GetBytes(LastMessegeIndetifer);

        internal TudpData([NotNull] byte[] dataWithIdentifer)
        {
            var identiferBytes = new byte[TudpUtils.IdentiferByteCount];
            for (int i = 0; i < identiferBytes.Length; i++)
            {
                identiferBytes[i] = dataWithIdentifer[i];
            }
            _identifer = identiferBytes;

            for (int i = identiferBytes.Length; i < dataWithIdentifer.Length; i++)
            {
                _data.Add(dataWithIdentifer[i]);
            }
        }

        internal byte[] GetDategramm()
        {
            return _data.ToArray();
        }

        internal byte[] GetIdentifer()
        {
            return _identifer;
        }

    }
}