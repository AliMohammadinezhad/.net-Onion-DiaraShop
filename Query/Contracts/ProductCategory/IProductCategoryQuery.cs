namespace Query.Contracts.ProductCategory;

public interface IProductCategoryQuery
{
    List<ProductCategoryQueryModel> GetProductCategoryQueryModels();
    List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();
}