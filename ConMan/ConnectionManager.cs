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
using ConnectionManager.Protocols;

namespace ConnectionManager
{
    public class ConnectionManager
    {
        public ConnectionManager(IConnectionProtocol protocol)
        {
            if (protocol == null) throw new ArgumentNullException();
            m_protocol = protocol;
        }

        //******************************
        // Attributes
        //******************************
        IConnectionProtocol m_protocol;
    }
}