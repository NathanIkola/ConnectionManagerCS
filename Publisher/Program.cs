using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Protocols;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);
            Connection connection = manager.GetConnection();

            // the job specifier that we are sending over
            byte id = 0;

            while (true)
            {
                // send out a message without a payload
                Message msg = new Message(id, null);
                connection.WriteMessage(msg);

                // increment the job specifier, skipping SUBSCRIBE and UNSUBSCRIBE
                // for simplicity
                ++id;
                if (id == 100) id += 2;

                // delay so we don't kill the server
                Thread.Sleep(500);
            }
        }
    }
}
