using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Protocols;
using System.Threading;
using ConnectionManagerCS.Listeners;
using System.Diagnostics;
using System.Net;

namespace Client
{
    class Program
    {
        const int SUBSCRIBE = 100;
        const int UNSUBSCRIBE = 101;

        static void Main(string[] args)
        {
            // establish a connection to the remote application
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);
            Connection conn = manager.GetConnection();

            // we want all messages that come our way
            conn.SubscribeToAll();

            // start out tracking evens
            byte mode = 0;
            Message m = new Message(SUBSCRIBE, new byte[] { mode });
            conn.WriteMessage(m);

            while(true)
            {
                // if there are no messages waiting to be read then poll for input
                while(conn.PendingMessages == 0)
                {
                    if(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        // unsubscribe from current
                        byte[] payload = { mode };
                        m = new Message(UNSUBSCRIBE, payload);
                        conn.WriteMessage(m);

                        // subscribe to other
                        if (mode == 0) mode = 1;
                        else mode = 0;

                        payload[0] = mode;
                        m = new Message(SUBSCRIBE, payload);
                        conn.WriteMessage(m);
                    }
                }

                // display the contents of the received message
                m = conn.ReadMessage();
                Console.WriteLine(String.Format("Received message with job specifier of {0}", m.JobSpecifier));
            }
        }
    }
}
