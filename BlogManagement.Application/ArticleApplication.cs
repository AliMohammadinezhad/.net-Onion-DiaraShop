using BlogManagement.Application.Contract.Article;
using Framework.Application;

namespace BlogManagement.Application;

public class ArticleApplication : IArticleApplication
{
    public OperationResult Create(CreateArticle command)
    {
        throw new NotImplementedException();
    }

    public OperationResult Edit(EditArticle command)
    {
        throw new NotImplementedException();
    }

    public EditArticle GetDetails(long id)
    {
        throw new NotImplementedException();
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        throw new NotImplementedException();
    }
}