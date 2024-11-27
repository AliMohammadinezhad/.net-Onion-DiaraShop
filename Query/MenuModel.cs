using Query.Contracts.ArticleCategory;
using Query.Contracts.ProductCategory;

namespace Query;

public class MenuModel
{
    public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
    public List<ProductCategoryQueryModel> productCategories { get; set; }
}