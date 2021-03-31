using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Protocols;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);
            Message msg = new Message(5, 2, null);
            manager.WriteMessage(msg);
            Thread.Sleep(100000);
        }
    }
}
