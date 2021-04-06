//************************************************
// ConnectionNotReadyException.cs
//
// Thrown when the connection being written to
// or read from was not set up and ready prior to
// the read or write call.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;

namespace ConnectionManagerCS.Exceptions
{
    public class ConnectionNotReadyException : Exception
    {
        public ConnectionNotReadyException(string msg) : base(msg) { }
    }
}
