

using Models.Enums;

namespace ClientApp
{
    public class MathRequest
    {
        public string? TypeOfRequest { get; set; }
        public decimal[]? Numbers { get; set; } = new decimal[2];
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
