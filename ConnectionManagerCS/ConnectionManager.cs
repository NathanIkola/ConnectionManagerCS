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

        public Message ReadMessage()
        {
            byte[] header = Protocol.ReadBytes(5);

            // per the framework spec: bytes 1-4 are the payload size and transaction ID
            int payloadSize = BitConverter.ToInt32(header, 1);
            // byte 1 is the transaction ID, so take bytes 2-4 as the payload size
            payloadSize = IPAddress.NetworkToHostOrder(payloadSize) & 0x00FFFFFF;
            byte[] payload = Protocol.ReadBytes(payloadSize);

            byte[] messageBytes = new byte[header.Length + payloadSize];
            header.CopyTo(messageBytes, 0);
            payload.CopyTo(messageBytes, header.Length);

            return Message.ParseNetworkBytes(messageBytes);
        }

        public void WriteMessage(Message msg)
        {
            byte[] msgBytes = msg.GetNetworkBytes();
            Protocol.WriteBytes(msgBytes);
        }

        //******************************
        // Attributes
        //******************************
        private IConnectionProtocol Protocol { get; set; }
        private byte[] Fragment { get; set; }
    }
}