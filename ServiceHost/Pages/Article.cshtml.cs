using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contracts.Article;
using Query.Contracts.ArticleCategory;

namespace ServiceHost.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleQueryModel Article;
        public List<ArticleQueryModel> LatestArticles;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _categoryQuery;
        public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery categoryQuery)
        {
            _articleQuery = articleQuery;
            _categoryQuery = categoryQuery;
        }

        public void OnGet(string articleSlug)
        {
            ArticleCategories = _categoryQuery.GetArticleCategories();
            LatestArticles = _articleQuery.LatestArticles();
            Article = _articleQuery.GetArticleDetails(articleSlug);
        }
    }
}
