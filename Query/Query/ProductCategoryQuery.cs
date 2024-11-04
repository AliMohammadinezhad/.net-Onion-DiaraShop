using DiscountManagement.Infrastructure.EfCore;
using Framework.Application;
using InventoryManagement.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Product;
using Query.Contracts.ProductCategory;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ApplicationDbContext _context;
    private readonly InventoryContext _inventoryContext;
    private readonly DiscountContext _discountContext;

    public ProductCategoryQuery(ApplicationDbContext context, InventoryContext inventoryContext,
        DiscountContext discountContext)
    {
        _context = context;
        _inventoryContext = inventoryContext;
        _discountContext = discountContext;
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
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice })
            .ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToList();
        var categories = _context.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Products = MapProducts(x.Products)
            }).ToList();

        foreach (var product in categories.SelectMany(category => category.Products))
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory == null) continue;
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                if (discount == null) continue;
                var discountRate = discount.DiscountRate;
                product.DiscountRate = discountRate;
                product.HasDiscount = discountRate > 0;

                var discountAmount = Math.Round(discountRate * price / 100);
                product.PriceWithDiscount = (price - discountAmount).ToMoney();
            }


        }


        return categories;
    }

    private static List<ProductQueryModel> MapProducts(List<Product> products)
    {
        return products.Select(product => new ProductQueryModel()
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
            })
            .ToList();
    }
}