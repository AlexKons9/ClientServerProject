using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class LogInfoDTO
    {
        [JsonProperty("username")]
        public string? UserName { get; set; }
        [JsonProperty("ip")]
        public string? IP { get; set; }

    }
}
