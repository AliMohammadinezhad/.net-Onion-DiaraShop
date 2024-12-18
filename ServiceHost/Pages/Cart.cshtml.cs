using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Newtonsoft.Json;
using Query.Contracts.Product;
using ShopManagement.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {


        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";

        private readonly IProductQuery _productQuery;
        public CartModel(IProductQuery productQuery)
        {
            CartItems = new List<CartItem>();
            _productQuery = productQuery;
        }
        public void OnGet()
        {
            var value = Request.Cookies[CookieName];
            if (value == null) return;
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value) ?? [];
            foreach (var item in cartItems)
                item.TotalItemPrice = item.Count * item.UnitPrice;

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }

        public IActionResult OnGetRemoveFromCart(long cartItemId)
        {
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            if (value == null) return RedirectToPage("/Index");
            List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value) ?? [];
            CartItem itemToRemove = cartItems.FirstOrDefault(x => x.Id == cartItemId) ?? throw new NullReferenceException();
            cartItems.Remove(itemToRemove);
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(2),
                IsEssential = true,
                Domain = "localhost",
            };
            var updatedCartItem = JsonConvert.SerializeObject(cartItems);
            Response.Cookies.Append(CookieName, updatedCartItem, options);
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGoToCheckOut()
        {
            var value = Request.Cookies[CookieName];
            if (value == null) 
                return RedirectToPage("/index");
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value) ?? [];
            if (cartItems == null) return RedirectToPage("/index");
            foreach (var item in cartItems)
            {
                item.TotalItemPrice = item.Count * item.UnitPrice;
            }

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
            return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout");
        }
    }
}
