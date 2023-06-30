

namespace ClientApp
{
    public class Request
    {
        public string? TypeOfRequest { get; set; }
        public int[]? Numbers { get; set; } = new int[2];
        public decimal Result { get; set; }



        public RequestType ParseRequestType(string typeOfRequest)
        {
            try
            {
                if (Enum.TryParse(typeOfRequest, out RequestType requestType))
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
