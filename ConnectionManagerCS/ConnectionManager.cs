//************************************************
// ConMan.cs
//
// The class responsible for managing a single
// connection between two computers over a single
// internet protocol.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Collections.Generic;
using System.Net;
using ConnectionManager.Protocols;

namespace ConnectionManager
{
    public class ConnectionManager
    {
        public ConnectionManager(IConnectionProtocol protocol)
        {
            Fragment = new byte[0];
            if (protocol == null) throw new ArgumentNullException();
            Protocol = protocol;
        }

        public Message Read()
        {
            byte[] header = new byte[5];
            if(Fragment.Length < header.Length)
            {
                byte[] secondFragment = Protocol.ReadMessage(header.Length - Fragment.Length);
                for(int _byte = 0; _byte < header.Length; ++_byte)
                {
                    if (_byte < Fragment.Length) header[_byte] = Fragment[_byte];
                    else header[_byte] = secondFragment[_byte - Fragment.Length];
                }
                Fragment = new byte[0];
            }
            else
            {
                for (int _byte = 0; _byte < header.Length; ++_byte)
                    header[_byte] = Fragment[_byte];

                byte[] resizedFragment = new byte[Fragment.Length - header.Length];
                for (int _byte = header.Length; _byte < Fragment.Length; ++_byte)
                    resizedFragment[_byte - header.Length] = Fragment[_byte];
                Fragment = resizedFragment;
            }

            int payloadSize = BitConverter.ToInt32(header, 1);
            payloadSize = IPAddress.NetworkToHostOrder(payloadSize) & 0xFFFFFF;
            byte[] payload = new byte[payloadSize];

            if (Fragment.Length < payloadSize)
            {
                byte[] secondFragment = Protocol.ReadMessage(payloadSize - Fragment.Length);
                for(int _byte = 0; _byte < payloadSize; ++_byte)
                {
                    if (_byte < Fragment.Length) payload[_byte] = Fragment[_byte];
                    else payload[_byte] = secondFragment[_byte - Fragment.Length];
                }
                Fragment = new byte[0];
            }
            else
            {
                for (int _byte = 0; _byte < payloadSize; ++_byte)
                    payload[_byte] = Fragment[_byte];

                byte[] resizedFragment = new byte[Fragment.Length - payloadSize];
                for (int _byte = payloadSize; _byte < Fragment.Length; ++_byte)
                    resizedFragment[_byte - payloadSize] = Fragment[_byte];
                Fragment = resizedFragment;
            }

            byte[] messageBytes = new byte[header.Length + payloadSize];
            header.CopyTo(messageBytes, 0);
            payload.CopyTo(messageBytes, header.Length);

            return Message.ParseNetworkBytes(messageBytes);
        }

        //******************************
        // Attributes
        //******************************
        private IConnectionProtocol Protocol { get; set; }
        private byte[] Fragment { get; set; }
    }
}