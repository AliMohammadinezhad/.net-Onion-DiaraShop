using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Query.Contracts;
using Query.Contracts.Product;
using ShopManagement.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        public List<CartItem> CartItem { get; set; }

        public const string CookieName = "cart-items";
        private readonly ICartCalculatorService _cartCalculatorService;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService, IProductQuery productQuery)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
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
            
            if(result.Any(x => !x.IsInStock))
                return RedirectToPage("./Cart");


            return RedirectToPage("./Checkout");
        }
    }
}
