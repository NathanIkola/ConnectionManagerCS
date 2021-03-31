using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS.Protocols;

namespace ConnectionManagerCS.Listeners
{
    public interface IListener
    {
        void Start();
        void Stop();
        List<ConnectionManager> Clients { get; }
    }
}
