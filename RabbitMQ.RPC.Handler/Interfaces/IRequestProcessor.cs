using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.RPC.Handler.Interfaces
{
    public interface IRequestProcessor
    {
        public GeneralTypeOfRequest Type { get; }
        Task<object> ProcessRequest(object request);
    }
}
