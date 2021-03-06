//************************************************
// IConnectionProtocol.cs
//
// Interface for all connection protocols
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

namespace ConnectionManagerCS.Protocols
{
    public interface IConnectionProtocol
    {
        void WriteBytes(byte[] messageBytes);
        byte[] ReadBytes(int messageLength);
        bool IsAlive();
        int MaxSupportedSize { get; }
    }
}
