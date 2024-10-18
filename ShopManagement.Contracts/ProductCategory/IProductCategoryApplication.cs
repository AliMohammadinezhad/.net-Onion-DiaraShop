
namespace ShopManagement.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    void Create(CreateProductCategory command);
    void Edit(EditProductCategory command);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel  command);
    Domain.ProductCategoryAgg.ProductCategory GetDetails(long id);
}