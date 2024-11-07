namespace Query.Contracts.ProductCategory;

public interface IProductCategoryQuery
{
    ProductCategoryQueryModel GetProductCategoryWithProducts(string slug);
    List<ProductCategoryQueryModel> GetProductCategoryQueryModels();
    List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();
}