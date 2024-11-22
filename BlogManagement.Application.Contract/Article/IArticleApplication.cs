using Framework.Application;

namespace BlogManagement.Application.Contract.Article;

public interface IArticleApplication
{
    OperationResult Create(CreateArticle command);
    OperationResult Edit(EditArticle command);
    EditArticle GetDetails(long  id);
    List<ArticleViewModel> Search(ArticleSearchModel searchModel);
}