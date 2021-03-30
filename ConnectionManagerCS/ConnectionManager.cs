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
using ConnectionManager.Protocols;

namespace ConnectionManager
{
    public class ConnectionManager
    {
        public ConnectionManager(IConnectionProtocol protocol)
        {
            if (protocol == null) throw new ArgumentNullException();
            Protocol = protocol;
        }

        public Message Read()
        {
            return null;
        }

        //******************************
        // Attributes
        //******************************
        private IConnectionProtocol Protocol { get; set; }
        private List<byte> Fragment { get; set; }
    }
}