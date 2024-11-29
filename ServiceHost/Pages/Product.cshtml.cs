using CommentManagement.Contract.Comment;
using CommentManagement.Infrastructure.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.Product;

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
            Product = _productQuery.GetProductDetails(productSlug);
        }

        public RedirectToPageResult OnPost(AddComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = _commentApplication.Add(command);
            return RedirectToPage("/Product", new { productSlug = productSlug });
        }
    }
}
