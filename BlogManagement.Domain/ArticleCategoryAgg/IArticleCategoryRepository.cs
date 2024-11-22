using BlogManagement.Application.Contract.ArticleCategory;
using Framework.Domain;

namespace BlogManagement.Domain.ArticleCategoryAgg;

public interface IArticleCategoryRepository : IRepository<long, ArticleCategory>
{
    string GetSlugBy(long id);
    EditArticleCategory GetDetails(long id);
    List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel  searchModel);
}