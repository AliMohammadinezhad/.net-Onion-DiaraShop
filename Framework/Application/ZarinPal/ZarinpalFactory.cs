using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Framework.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;

        public string Prefix { get; set; }
        private string MerchantId { get; }

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            Prefix = _configuration.GetSection("payment")["method"];
            MerchantId = _configuration.GetSection("payment")["merchant"];
        }

        public PaymentResponse CreatePaymentRequest(
            string amount,
            string mobile,
            string email,
            string description,
            long orderId)
        {
            amount = amount.Replace(",", "");
            if (!int.TryParse(amount, out var finalAmount) || finalAmount <= 0)
            {
                throw new ArgumentException("Invalid amount", nameof(amount));
            }

            var siteUrl = _configuration.GetSection("payment")?["siteUrl"];
            if (string.IsNullOrEmpty(siteUrl))
            {
                throw new InvalidOperationException("Site URL is not configured.");
            }

            var MerchantId = _configuration.GetSection("payment")?["merchant"];
            if (string.IsNullOrEmpty(MerchantId))
            {
                throw new InvalidOperationException("Merchant ID is not configured.");
            }

            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/v4/payment/request.json");
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("Content-Type", "application/json");
            var body = new PaymentRequest
            {
                Mobile = mobile,
                CallbackURL = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
                Description = description,
                Email = email,
                Amount = finalAmount * 10,
                MerchantID = MerchantId
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            request.AddJsonBody(jsonBody);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {response.Content}");
            }

            return JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
        }


        public VerificationResponse CreateVerificationRequest(string authority, string amount)
        {
            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/v4/payment/verify.json");
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("Content-Type", "application/json");

            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);

            var body = new VerificationRequest
            {
                Amount = finalAmount * 10,
                MerchantID = MerchantId,
                Authority = authority
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            request.AddJsonBody(jsonBody);
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<VerificationResponse>(response.Content);
        }
    }
}