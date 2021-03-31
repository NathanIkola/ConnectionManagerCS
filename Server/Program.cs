using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Listeners;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPListener listener = new TCPListener(3000);
            listener.Start();
            Thread.Sleep(500);
            foreach(ConnectionManager manager in listener.Clients)
            {
                Message msg = manager.ReadMessage();
                Console.WriteLine(String.Format("Message read with Job Specifier {0}, Transaction ID {1}", msg.JobSpecifier, msg.TransactionID));
            }
            Console.WriteLine("Done");
        }
    }
}
