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
        const int SUBSCRIBE = 100;
        const int UNSUBSCRIBE = 101;
        static void Main(string[] args)
        {
            // begin listening for clients
            IListener listener = new TCPListener(3000);
            listener.Start();

            while(true)
            {
                // listen for messages coming from the connected clients
                foreach(Connection conn in listener.Clients)
                {
                    // process all messages they have (if any at all)
                    while(conn.PendingMessages > 0)
                    {
                        Dispatch(conn.ReadMessage(), conn);
                    }
                }
            }
        }

        static void Dispatch(Message msg, Connection conn)
        {
            switch(msg.JobSpecifier)
            {
                case SUBSCRIBE:
                    {
                        // even
                        if (msg.Payload == null || msg.Payload[0] == 0)
                            EvenSubscribers.Add(conn);
                        else
                            OddSubscribers.Add(conn);
                        break;
                    }
                case UNSUBSCRIBE:
                    {
                        // even
                        if (msg.Payload == null || msg.Payload[0] == 0)
                            EvenSubscribers.Remove(conn);
                        else
                            OddSubscribers.Remove(conn);
                        break;
                    }
                default:
                    {
                        // even
                        if (msg.JobSpecifier % 2 == 0)
                            foreach (Connection c in EvenSubscribers) c.WriteMessage(msg);
                        else
                            foreach (Connection c in OddSubscribers) c.WriteMessage(msg);
                        break;
                    }
            }
        }

        static List<Connection> EvenSubscribers = new List<Connection>();
        static List<Connection> OddSubscribers = new List<Connection>();
    }
}
