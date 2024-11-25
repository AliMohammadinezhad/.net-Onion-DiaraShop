namespace Query.Contracts.ArticleCategory;

public interface IArticleCategoryQuery
{
    List<ArticleCategoryQueryModel> GetArticleCategories();
}