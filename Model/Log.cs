using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class Log
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfLog { get; set; } = DateTime.Now;
        public string? IP { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
