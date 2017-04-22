using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    internal sealed class TudpData
    {
        private const int LastMessegeIndetifer = -1;
        private const int ConnectionMessegeIndetifer = -2;

        private readonly List<byte> _data = new List<byte>();
        private readonly byte[] _identifer;
        private readonly int _identiferAtInt;

        internal readonly static byte[] LastMessege = BitConverter.GetBytes(LastMessegeIndetifer);
        internal readonly static byte[] ConnectionMessege = BitConverter.GetBytes(ConnectionMessegeIndetifer);

        internal TudpData([NotNull] byte[] dataWithIdentifer)
        {
            var identiferBytes = new byte[TudpUtils.IdentiferByteCount];
            for (int i = 0; i < identiferBytes.Length; i++)
            {
                identiferBytes[i] = dataWithIdentifer[i];
            }
            _identifer = identiferBytes;
            _identiferAtInt = BitConverter.ToInt32(identiferBytes, 0);

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

        internal bool IsConnectionMessege()
        {
            return _identiferAtInt == ConnectionMessegeIndetifer;
        }

        internal bool IsLastMessege()
        {
            return _identiferAtInt == LastMessegeIndetifer;
        }

        internal bool CompareIdentifer(int candidate)
        {
            return _identiferAtInt == candidate;
        }

        internal bool CompareIdentifer(byte[] candidate)
        {
            return _identiferAtInt == BitConverter.ToInt32(candidate, 0);
        }
    }
}