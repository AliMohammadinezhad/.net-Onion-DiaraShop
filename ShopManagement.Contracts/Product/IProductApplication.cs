using Framework.Application;

namespace ShopManagement.Contracts.Product;

public interface IProductApplication
{
    OperationResult Create(CreateProduct command);
    OperationResult Edit(EditProduct  command);
    void InStock(long id);
    void NotInStock(long id);
    EditProduct GetDetails(long id);
    List<ProductViewModel> Search(ProductSearchModel searchModel);

}