using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Models.ResponseModels
{
    public class Response<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
}
