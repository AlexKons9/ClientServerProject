using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RPC.Handler.Interfaces
{
    public interface IRPCServer
    {
        void StartListening();
        void StopListening();
    }
}
