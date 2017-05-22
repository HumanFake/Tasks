using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    internal sealed class TudpData
    {
        private const int LastMessageIdentifier = -1;
        private const int ConnectionMessageIdentifier = -2;

        /*
        �I�������|���x���u������ �|�y����, ���p�{-�{�p�{ ���p�x�}�u�� �~�u �y�x�r�u�����u�~. �N�p�����y�}�u�� �������|�u�t�~�y�u �N �q�p�z�� ���p�z�|�p
        �~�u ���q���x�p�~�� �q������ ���p�x�}�u�����} �� �t�p���p�s���p�}�}��.
        */
        private readonly List<byte> _data = new List<byte>();
        private readonly byte[] _identifier;
        private readonly int _identifierAtInt;

        internal static readonly byte[] LastMessage = BitConverter.GetBytes(LastMessageIdentifier);
        internal static readonly byte[] ConnectionMessage = BitConverter.GetBytes(ConnectionMessageIdentifier);

        internal TudpData([NotNull] byte[] dataWithIdentifier)
        {
            var identifierBytes = new byte[TudpUtils.IdentifierByteCount];
            for (int i = 0; i < identifierBytes.Length; i++)
            {
                identifierBytes[i] = dataWithIdentifier[i];
            }
            _identifier = identifierBytes;
            _identifierAtInt = BitConverter.ToInt32(identifierBytes, 0);

            for (int i = identifierBytes.Length; i < dataWithIdentifier.Length; i++)
            {
                _data.Add(dataWithIdentifier[i]);
            }
        }

        internal byte[] GetDatagram()
        {
            return _data.ToArray();
        }

        internal byte[] GetIdentifier()
        {
            return _identifier;
        }

        internal bool IsConnectionMessage()
        {
            return _identifierAtInt == ConnectionMessageIdentifier;
        }

        internal bool IsLastMessage()
        {
            return _identifierAtInt == LastMessageIdentifier;
        }

        internal bool CompareIdentifier(int candidate)
        {
            return _identifierAtInt == candidate;
        }

        internal bool CompareIdentifier(byte[] candidate)
        {
            return _identifierAtInt == BitConverter.ToInt32(candidate, 0);
        }
    }
}