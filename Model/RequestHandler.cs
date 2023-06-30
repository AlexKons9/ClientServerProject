using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RequestHandler<T> where T : class
    {
        public T Data { get; set; }
        public GeneralTypeOfRequest Type { get; set; }

        public RequestHandler(T data, GeneralTypeOfRequest type)
        {
            Data = data;
            Type = type;
        }

    }
}
