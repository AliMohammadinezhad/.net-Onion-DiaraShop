using Newtonsoft.Json;

namespace Framework.Application.ZarinPal
{
    public class VerificationRequest
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("merchant_id")]
        public string MerchantID { get; set; }
        [JsonProperty("authority")]
        public string Authority { get; set; }
    }
}
