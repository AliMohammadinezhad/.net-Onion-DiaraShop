namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository
{
    void Create(ProductCategory entity);
    ProductCategory Get(long id);
    List<ProductCategory> GetAll();
    bool Exists(string name);
    void Save();
}