namespace Framework.Application.ZarinPal
{

    public class PaymentResponse
    {
        public PaymentResponseData Data { get; set; }
        public class PaymentResponseData    
        {
            public int Status { get; set; }
            public string Authority { get; set; }
        }
    }
}
