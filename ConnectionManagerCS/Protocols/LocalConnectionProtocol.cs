using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS.Listeners;

namespace ConnectionManagerCS.Protocols
{
    public class LocalConnectionProtocol : IConnectionProtocol
    {
        public LocalConnectionProtocol()
        {
            Messages = new Queue<byte[]>();
        }

        public void WriteBytes(byte[] messageBytes)
        {
            Messages.Enqueue(messageBytes);
        }

        public byte[] ReadBytes(int messageLength)
        {
            return Messages.Dequeue();
        }

        public bool IsAlive()
        {
            return true;
        }


        private Queue<byte[]> Messages { get; set; }
    }
}
