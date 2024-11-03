using Microsoft.EntityFrameworkCore;
using Query.Contracts.Product;
using Query.Contracts.ProductCategory;
using ShopManagement.Domain.ProductAgg;
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

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        return _context.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
        {
            Id = x.Id,
            Name = x.Name,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug,
            Products = MapProducts(x.Products)
        }).ToList();
    }

    private static List<ProductQueryModel> MapProducts(List<Product> products)
    {
        var result = new List<ProductQueryModel>();
        foreach (var product in products)
        {
            var item = new ProductQueryModel()
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
            };
            result.Add(item);
        }

        return result;
    }
}