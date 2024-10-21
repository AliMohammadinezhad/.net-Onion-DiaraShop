using Framework.Application;

namespace ShopManagement.Contracts.Product;

public interface IProductApplication
{
    OperationResult Create(CreateProduct command);
    OperationResult Edit(EditProduct  command);
    OperationResult InStock(long id);
    OperationResult NotInStock(long id);
    EditProduct GetDetails(long id);
    List<ProductViewModel> GetProducts();
    List<ProductViewModel> Search(ProductSearchModel searchModel);

}