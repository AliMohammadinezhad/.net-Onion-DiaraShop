using Framework.Application;

namespace ShopManagement.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    OperationResult Create(CreateProductCategory command);
    OperationResult Edit(EditProductCategory command);
    List<ProductCategoryViewModel> GetProductCategories();
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel  command);
    EditProductCategory GetDetails(long id);
}