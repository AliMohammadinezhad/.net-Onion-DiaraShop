using BlogManagement.Application.Contract.Article;
using Framework.Domain;

namespace BlogManagement.Domain.ArticleAgg;

public interface IArticleRepository : IRepository<long, Article>
{
    EditArticle GetDetails(long id);
    List<ArticleViewModel> Search(ArticleSearchModel searchModel);
}