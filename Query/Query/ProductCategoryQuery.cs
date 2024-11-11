using System.Reflection.Metadata.Ecma335;
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

    public ProductCategoryQueryModel GetProductCategoryWithProducts(string slug)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice })
            .AsNoTracking().ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
            .AsNoTracking().ToList();
        var category = _context.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Products = MapProducts(x.Products),
                Description = x.Description,
                Keyword = x.Keyword,
                Slug = x.Slug,
                MetaDescription = x.MetaDescription,

            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        foreach (var product in category?.Products)
        {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

                if (productInventory == null) continue;
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);

                if (discount == null) continue;
                var discountRate = discount.DiscountRate;
                product.DiscountRate = discountRate;
                product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                product.HasDiscount = discountRate > 0;

                var discountAmount = Math.Round(discountRate * price / 100);
                product.PriceWithDiscount = (price - discountAmount).ToMoney();


        }


        return category;

    }

    public List<ProductCategoryQueryModel> GetProductCategoryQueryModels()
    {
        return _context.ProductCategories.Select(x => new ProductCategoryQueryModel
        {
            Picture = x.Picture,
            Name = x.Name,
            PictureAlt = x.PictureAlt,
            Id = x.Id,
            Slug = x.Slug,
            PictureTitle = x.PictureTitle
        }).AsNoTracking().ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice })
            .AsNoTracking().ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .AsNoTracking().ToList();
        var categories = _context.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Products = MapProducts(x.Products),
            }).AsNoTracking().ToList();

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
                CategorySlug = product.Category.Slug,
                Slug = product.Slug
            })
            .ToList();
    }
}