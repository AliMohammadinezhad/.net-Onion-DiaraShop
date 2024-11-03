using Query.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace Query.Contracts.ProductCategory;

public class ProductCategoryQueryModel
{
    public long Id { get; set; }
    public string Name { get;  set; }
    public string Picture { get;  set; }
    public string PictureAlt { get;  set; }
    public string PictureTitle { get;  set; }
    public string Slug { get;  set; }
    public List<ProductQueryModel> Products { get; set; }
}