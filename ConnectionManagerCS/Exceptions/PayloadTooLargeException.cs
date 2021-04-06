//************************************************
// PayloadTooLargeException.cs
//
// Thrown when the payload generated is larger
// than the allowed size.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;

namespace ConnectionManagerCS.Exceptions
{
    public class PayloadTooLargeException : Exception
    {
        public PayloadTooLargeException(string msg) : base(msg) { }
    }
}
