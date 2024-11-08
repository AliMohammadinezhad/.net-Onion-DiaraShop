using DiscountManagement.Infrastructure.EfCore;
using Framework.Application;
using InventoryManagement.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Product;
using Query.Contracts.ProductCategory;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Query;

public class ProductQuery : IProductQuery
{
    private readonly ApplicationDbContext _context;
    private readonly InventoryContext _inventoryContext;
    private readonly DiscountContext _discountContext;

    public ProductQuery(ApplicationDbContext context, InventoryContext inventoryContext,
        DiscountContext discountContext)
    {
        _context = context;
        _inventoryContext = inventoryContext;
        _discountContext = discountContext;
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var products = _context.Products
            .Include(x => x.Category)
            .Select(product => new ProductQueryModel()
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(6).ToList();

        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice })
            .AsNoTracking().ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .AsNoTracking().ToList();

        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            
            if (productInventory == null) continue;
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

        return products;
    }

    public List<ProductQueryModel> Search(string value)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice })
            .AsNoTracking().ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
            .AsNoTracking().ToList();
        var query = _context.Products
            .Include(x => x.Category)
            .Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
                CategorySlug = product.Category.Slug,
                Slug = product.Slug,
                ShortDescription = product.ShortDescription

            }).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(value))
            query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));

        var products = query.OrderByDescending(x => x.Id).ToList();

        foreach (var product in products)
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


        return products;
    }

}