namespace Query.Contracts.ArticleCategory;

public interface IArticleCategoryQuery
{
    ArticleCategoryQueryModel getArticleCategoryBySlug(string slug);
    List<ArticleCategoryQueryModel> GetArticleCategories();
}