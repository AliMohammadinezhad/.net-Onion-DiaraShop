using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Infrastructure.Configuration.Permissions;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Pages.Blog.ArticleCategories
{
    public class IndexModel : PageModel
    {
        public ArticleCategorySearchModel SearchModel;

        public List<ArticleCategoryViewModel> ArticleCategory;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public IndexModel(IArticleCategoryApplication articleCategoryApplication)
        {
            _articleCategoryApplication = articleCategoryApplication;
        }

        [NeedsPermission(BlogPermissions.ListArticleCategories)]
        public void OnGet(ArticleCategorySearchModel searchModel)
        {
            ArticleCategory = _articleCategoryApplication.Search(searchModel);
        }

        [NeedsPermission(BlogPermissions.CreateArticleCategory)]
        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateArticleCategory());
        }

        [NeedsPermission(BlogPermissions.CreateArticleCategory)]
        public IActionResult OnPostCreate(CreateArticleCategory command)
        {
            var result = _articleCategoryApplication.Create(command);
            return new JsonResult(result);
        }

        [NeedsPermission(BlogPermissions.EditArticleCategories)]
        public IActionResult OnGetEdit(long id)
        {
            var articleCategory = _articleCategoryApplication.GetDetails(id);
            return Partial("./Edit", articleCategory);
        }

        [NeedsPermission(BlogPermissions.EditArticleCategories)]
        public JsonResult OnPostEdit(EditArticleCategory command)
        {
            var result = _articleCategoryApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
