using BlogManagement.Application.Contract.Article;
using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Infrastructure.Configuration.Permissions;
using DiscountManagement.infrastructure.Configuration.Permissions;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles
{
    public class CreateModel : PageModel
    {
        public CreateArticle Command;
        public SelectList ArticleCategories;

        private readonly IArticleCategoryApplication _articleCategoryApplication;
        private readonly IArticleApplication _articleApplication;
        public CreateModel(IArticleCategoryApplication articleCategoryApplication, IArticleApplication articleApplication)
        {
            _articleCategoryApplication = articleCategoryApplication;
            _articleApplication = articleApplication;
        }

        [NeedsPermission(BlogPermissions.CreateArticle)]
        public void OnGet()
        {
            ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
        }

        [NeedsPermission(BlogPermissions.CreateArticle)]
        public IActionResult OnPost(CreateArticle command)
        {
            _articleApplication.Create(command);
            return RedirectToPage("./Index");
        }
    }
}
