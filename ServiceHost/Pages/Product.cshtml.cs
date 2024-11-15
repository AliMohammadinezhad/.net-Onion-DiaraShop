using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.Product;
using ShopManagement.Contracts.Comment;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;
        public ProductQueryModel Product;
        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }

        public void OnGet(string productSlug)
        {
            Product = _productQuery.GetDetails(productSlug);
        }

        public RedirectToPageResult OnPost(AddComment command, string productSlug)
        {
            var result = _commentApplication.Add(command);
            return RedirectToPage("/Product", new {productSlug= productSlug});
        }
    }
}
