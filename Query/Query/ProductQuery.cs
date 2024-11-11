using DiscountManagement.Infrastructure.EfCore;
using Framework.Application;
using InventoryManagement.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.Product;
using ShopManagement.Domain.ProductPictureAgg;
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

    public ProductQueryModel GetDetails(string slug)
    {
        var product = _context.Products
            .Include(x => x.Category)
            .Include(x => x.ProductPictures)
            .Select(product => new ProductQueryModel()
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
                CategorySlug = product.Category.Slug,
                Slug = product.Slug,
                Code = product.Code,
                Description = product.Description,
                Keyword = product.Keyword,
                MetaDescription = product.MetaDescription,
                ShortDescription = product.ShortDescription,
                Pictures = MapProductPictures(product.ProductPictures)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice, x.InStock })
            .AsNoTracking().ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
            .AsNoTracking().ToList();

        if (product == null) return new ProductQueryModel();

            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory == null) return product;
            var price = productInventory.UnitPrice;
            product.Price = price.ToMoney();
            product.IsInStock = productInventory.InStock;

            var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
         
            if (discount == null) return product;

            var discountRate = discount.DiscountRate;
            product.DiscountRate = discountRate;
            product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
            product.HasDiscount = discountRate > 0;

            var discountAmount = Math.Round(discountRate * price / 100);
            product.PriceWithDiscount = (price - discountAmount).ToMoney();

            return product;
    }

    private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
    {
        return pictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemoved,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            })
            .Where(x => !x.IsRemoved)
            .ToList();
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
                CategorySlug = product.Category.Slug,
                Slug = product.Slug
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