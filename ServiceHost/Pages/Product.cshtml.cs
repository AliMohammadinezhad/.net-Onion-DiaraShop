using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.Product;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        public ProductQueryModel Product;
        public ProductModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string productSlug)
        {
            Product = _productQuery.GetDetails(productSlug);
        }
    }
}
