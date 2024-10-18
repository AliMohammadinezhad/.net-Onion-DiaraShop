using System.Linq.Expressions;
using Framework.Domain;
using ShopManagement.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{
    EditProductCategory GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel command);
}