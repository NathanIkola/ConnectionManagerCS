using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Listeners;
using System.Threading;
using System.Diagnostics;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // begin listening for clients
            //IListener listener = new TCPListener(3000);
            UDPListener listener = new UDPListener(3000);
            listener.Start();

            // wait for the client to get a message to us
            while (listener.Clients[0].PendingMessages == 0) { }

            // start tracking time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // read from all connected clients
            foreach (Connection client in listener.Clients)
            {
                while(client.PendingMessages > 0)
                {
                    Message msg = client.ReadMessage();
                    Console.WriteLine(
                        String.Format("Message read with Job Specifier {0}, Transaction ID {1}, payload size {2}",
                                        msg.JobSpecifier, msg.TransactionID, msg.PayloadSize));
                }
            }
            stopwatch.Stop();
            Console.WriteLine(String.Format("{0}ms elapsed", stopwatch.ElapsedMilliseconds));

            Console.WriteLine("Done");
            Thread.Sleep(10000);
        }
    }
}
