

using Models.Enums;
using Newtonsoft.Json;

namespace ClientApp
{
    public class MathRequest
    {
        [JsonProperty("typeOfRequest")]
        public string? TypeOfRequest { get; set; }
        [JsonProperty("numbers")]
        public decimal[]? Numbers { get; set; } = new decimal[2];
        [JsonProperty("result")]
        public decimal Result { get; set; }



        public MathRequestType ParseRequestType(string typeOfRequest)
        {
            try
            {
                if (Enum.TryParse(typeOfRequest, out MathRequestType requestType))
                {
                    return requestType;
                }

                throw new Exception("Not Valid Request Type");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
