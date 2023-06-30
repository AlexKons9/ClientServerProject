using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class Log
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string? UserName { get; set; }
        [JsonProperty("details")]
        public string Details { get; set; }
        [JsonProperty("dateoflog")]
        public DateTime DateOfLog { get; set; } = DateTime.Now;
        [JsonProperty("ip")]
        public string? IP { get; set; }
        [JsonProperty("duration")]
        public TimeSpan? Duration { get; set; }
    }
}
