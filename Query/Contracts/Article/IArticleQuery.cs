namespace Query.Contracts.Article;

public interface IArticleQuery
{
    List<ArticleQueryModel> LatestArticles();
}