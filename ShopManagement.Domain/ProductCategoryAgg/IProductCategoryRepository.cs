using System.Linq.Expressions;
using Framework.Domain;
using ShopManagement.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{
    List<ProductCategoryViewModel> GetProductCategories();
    EditProductCategory GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel command);
}