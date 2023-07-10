using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class FetchLogsDTO
    {
        [JsonProperty("logInfo")]
        public LogInfoDTO LogInfo { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
    }
}
