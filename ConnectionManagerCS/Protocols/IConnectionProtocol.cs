//************************************************
// IConnectionProtocol.cs
//
// Interface for all connection protocols
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using ConnectionManager;

namespace ConnectionManager.Protocols
{
    public interface IConnectionProtocol
    {
        void WriteBytes(byte[] messageBytes);
        byte[] ReadBytes(int messageLength);
    }
}
