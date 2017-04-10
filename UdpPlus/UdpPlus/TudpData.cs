using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    internal sealed class TudpData
    {
        private const int LastMessegeIndetifer = -1;

        private readonly byte[] _identifer;
        private readonly List<byte> _data = new List<byte>();
        internal readonly static TudpData LastMessege = new TudpData(BitConverter.GetBytes(LastMessegeIndetifer));

        internal TudpData([NotNull] byte[] dataWithIdentifer)
        {
            var identiferBytes = new byte[TudpUtils.IdentiferByteCount];
            for (int i = 0; i < identiferBytes.Length; i++)
            {
                identiferBytes[i] = dataWithIdentifer[i];
            }
            _identifer = identiferBytes;

            for (int i = TudpUtils.IdentiferByteCount; i < dataWithIdentifer.Length; i++)
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