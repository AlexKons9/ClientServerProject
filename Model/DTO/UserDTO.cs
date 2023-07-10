using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UserDTO
    {
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("logInfo")]
        public LogInfoDTO LogInfo { get; set; }
    }
}
