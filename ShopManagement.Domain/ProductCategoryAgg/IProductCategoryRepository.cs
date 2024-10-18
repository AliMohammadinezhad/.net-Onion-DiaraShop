using System.Linq.Expressions;
using ShopManagement.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository
{
    void Create(ProductCategory entity);
    ProductCategory Get(long id);
    List<ProductCategory> GetAll();
    bool Exists(Expression<Func<ProductCategory, bool>> expression);
    void Save();
    EditProductCategory GetDetails(long id);
    List<ProductCategoryViewModel> Search(ProductCategorySearchModel command);
}