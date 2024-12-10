using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Query.Contracts;
using ShopManagement.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        public List<CartItem> CartItem { get; set; }

        public const string CookieName = "cart-items";
        private readonly ICartCalculatorService _cartCalculatorService;
        public CheckoutModel(ICartCalculatorService cartCalculatorService)
        {
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
        }
    }
}
