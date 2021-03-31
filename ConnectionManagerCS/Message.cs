//************************************************
// Message.cs
//
// This is the class that will generate the
// header for my networking framework and will
// encapsulate the entire message to be sent
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Net;
using System.Runtime.InteropServices;
using ConnectionManagerCS.Exceptions;

namespace ConnectionManagerCS
{
    public class Message
    {
        public Message(byte jobSpecifier, byte transactionID, byte[] payload)
        {
            JobSpecifier = jobSpecifier;
            TransactionID = transactionID;
            if (payload != null)
                Payload = payload;
            else
                Payload = new byte[0];
        }

        public Message(byte[] payload) : this(0, 0, payload) { }

        public byte[] GetNetworkBytes()
        {
            byte[] messageBytes = new byte[MessageSize];
            messageBytes[0] = JobSpecifier;

            uint payloadSize = PayloadSize;
            if (payloadSize > MaxPayloadSize)
                throw new PayloadTooLargeException(payloadSize.ToString());

            // the first byte of the payload size is the transaction ID
            payloadSize += (uint)(TransactionID << 24);
            payloadSize = (uint)IPAddress.HostToNetworkOrder((int)payloadSize);

            byte[] payloadSizeBytes = BitConverter.GetBytes(payloadSize);
            payloadSizeBytes.CopyTo(messageBytes, 1);

            Payload.CopyTo(messageBytes, 5);

            return messageBytes;
        }

        public static Message ParseNetworkBytes(byte[] networkMsgBytes)
        {
            if (networkMsgBytes == null) throw new ArgumentNullException();
            if (networkMsgBytes.Length < 5)
                throw new ArgumentException(String.Format("networkMsgBytes was only {0} bytes", networkMsgBytes.Length));

            byte jobSpecifier = networkMsgBytes[0];

            uint payloadSize = BitConverter.ToUInt32(networkMsgBytes, 1);
            payloadSize = (uint)IPAddress.NetworkToHostOrder((int)payloadSize);

            byte transactionID = (byte)(payloadSize >> 24);
            payloadSize = payloadSize & ~(0xFF << 24);

            if (payloadSize + 5 != networkMsgBytes.Length) 
                throw new ArgumentException(String.Format("networkMsgBytes was {0} bytes, {0} expected", 
                    networkMsgBytes.Length, payloadSize+5));

            byte[] payload = new byte[payloadSize];
            for (int i = 0; i < payloadSize; ++i)
                payload[i] = networkMsgBytes[i + 5];

            return new Message(jobSpecifier, transactionID, payload);
        }

        //******************************
        // Attributes
        //******************************
        public byte JobSpecifier { get; set; }

        public byte TransactionID { get; set; }

        public byte[] Payload { get; set; }

        public uint PayloadSize 
        { 
            get 
            {
                if (Payload == null) return 0;
                return (uint)Payload.Length;
            } 
        }

        public static uint MaxPayloadSize { get { return (1 << 24) - 1; } }

        public uint MessageSize
        {
            get
            {
                return (uint)(Marshal.SizeOf(JobSpecifier) + Marshal.SizeOf(PayloadSize) + PayloadSize);
            }
        }
    }
}
