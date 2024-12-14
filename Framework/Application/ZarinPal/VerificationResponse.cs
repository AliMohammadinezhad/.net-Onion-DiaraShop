using Newtonsoft.Json;

namespace Framework.Application.ZarinPal
{
    public class VerificationResponse
    {
        [JsonProperty("data")]
        public VerificationResponseData Data { get; set; }
        public class VerificationResponseData
        {
            [JsonProperty("ref_id")]
            public long RefID { get; set; }
            [JsonProperty("code")]
            public int Status { get; set; }
        }
    }
}
