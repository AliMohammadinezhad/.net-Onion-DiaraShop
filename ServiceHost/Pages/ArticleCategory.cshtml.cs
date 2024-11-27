using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.Article;
using Query.Contracts.ArticleCategory;

namespace ServiceHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> LatestArticles;
        private readonly IArticleCategoryQuery _articleCategory;
        private readonly IArticleQuery _articleQuery;
        public ArticleCategoryModel(IArticleCategoryQuery articleCategory, IArticleQuery articleQuery)
        {
            _articleCategory = articleCategory;
            _articleQuery = articleQuery;
        }

        public void OnGet(string articleCategorySlug)
        {
            ArticleCategory = _articleCategory.getArticleCategoryBySlug(articleCategorySlug);
            ArticleCategories = _articleCategory.GetArticleCategories();
            LatestArticles = _articleQuery.LatestArticles();
        }
    }
}
