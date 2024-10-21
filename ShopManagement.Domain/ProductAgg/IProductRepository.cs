using Framework.Domain;
using ShopManagement.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg;

public interface IProductRepository : IRepository<long, Product>
{
    EditProduct GetDetails(long id);
    List<ProductViewModel> GetProducts();
    List<ProductViewModel> Search(ProductSearchModel  searchModel);
}