using System.Globalization;
using Framework.Application;
using Framework.Application.ZarinPal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Query.Contracts;
using Query.Contracts.Product;
using ShopManagement.Contracts.Order;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        public List<CartItem> CartItem { get; set; }

        public const string CookieName = "cart-items";

        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(
            IAuthHelper authHelper,
            ICartService cartService,
            IProductQuery productQuery,
            IZarinPalFactory zarinPalFactory,
            IOrderApplication orderApplication,
            ICartCalculatorService cartCalculatorService
            )
        {
            _authHelper = authHelper;
            _cartService = cartService;
            _productQuery = productQuery;
            _zarinPalFactory = zarinPalFactory;
            _orderApplication = orderApplication;
            _cartCalculatorService = cartCalculatorService;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);
        }

        public IActionResult OnGetPay()
        {
            var cart = _cartService.Get();
            var result = _productQuery.CheckInventoryStatus(cart.CartItems);

            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("./Cart");

            var orderId = _orderApplication.PlaceOrder(cart);
            var account = _authHelper.CurrentAccountInfo();

            var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
                cart.PayAmount.ToString(),
                "",
                "",
                "خرید تستی",
                orderId
                );

            return Redirect($"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Data.Authority}");
        }

        public IActionResult OnGetCallback(
            [FromQuery] string authority,
            [FromQuery] string status,
            [FromQuery] long oId
            )
        {
            var orderAmount = _orderApplication.GetAmountBy(oId);
            var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority, orderAmount.ToString());

            var result = new PaymentResult();
            switch (status)
            {
                case "OK" when verificationResponse.Data.Status == 100:
                {
                    var issueTrackingNumber = _orderApplication.PaymentSucceeded(oId, verificationResponse.Data.RefID);
                    Response.Cookies.Delete(CookieName);
                    result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNumber);
                    return RedirectToPage("./PaymentResult", result);
                }
                case "OK" when verificationResponse.Data.Status == 101:
                    result = result.Failed("پرداخت با موفقیت انجام شد اما شما دوباره سعی به پرداخت کردید!");
                    return RedirectToPage("./PaymentResult", result);
                default:
                    result = result.Failed("پرداخت با موفقیت انجام نشد در صورت کسر وجه از حساب مبلغ طی 24 ساعت به حساب شما برگردانده خواهد شد.");
                    return RedirectToPage("./PaymentResult", result);
            }
        }
    }
}
