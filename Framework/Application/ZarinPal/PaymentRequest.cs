using Newtonsoft.Json;

namespace Framework.Application.ZarinPal
{
    public class PaymentRequest
    {
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("callback_url")]
        public string CallbackURL { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("merchant_id")]
        public string MerchantID { get; set; }
    }
}
