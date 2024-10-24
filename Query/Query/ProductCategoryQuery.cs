using Query.Contracts.ProductCategory;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ApplicationDbContext _context;

    public ProductCategoryQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ProductCategoryQueryModel> GetProductCategoryQueryModels()
    {
        return _context.ProductCategories.Select(x => new ProductCategoryQueryModel
        {
            Picture = x.Picture,
            Name = x.Name,
            PictureAlt = x.PictureAlt,
            Id = x.Id,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug
        }).ToList();
    }
}