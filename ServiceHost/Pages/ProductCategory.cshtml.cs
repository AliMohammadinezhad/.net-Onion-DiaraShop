using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.ProductCategory;

namespace ServiceHost.Pages
{
    public class ProductCategoryModel : PageModel
    {
        public ProductCategoryQueryModel ProductCategory;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public void OnGet(string categorySlug)
        {
            ProductCategory = _productCategoryQuery.GetProductCategoryWithProducts(categorySlug);
        }
    }
}
