using Framework.Domain;
using ShopManagement.Contracts.ProductPicture;

namespace ShopManagement.Domain.ProductPictureAgg;

public interface IProductPictureRepository : IRepository<long, ProductPicture>
{
    EditProductPicture GetDetails(long id);
    List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel);
}