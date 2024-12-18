using InventoryManagement.Infrastructure.EfCore;
using Query.Contracts.Inventory;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Query;

public class InventoryQuery : IInventoryQuery
{
    private readonly InventoryContext  _inventoryContext;
    private readonly ApplicationDbContext _applicationDbContext;

    public InventoryQuery(InventoryContext inventoryContext, ApplicationDbContext applicationDbContext)
    {
        _inventoryContext = inventoryContext;
        _applicationDbContext = applicationDbContext;
    }

    public StockStatus CheckStock(IsInStock command)
    {
        var inventory = _inventoryContext.Inventory
            .FirstOrDefault(x=> x.ProductId == command.ProductId);

        if (inventory == null || inventory.CalculateCurrentCount() < command.Count)
        {
            var product = _applicationDbContext.Products.Select(x => new { x.Name, x.Id })
                .FirstOrDefault(x => x.Id == command.ProductId);

            return new StockStatus
            {
                IsStock = false,
                ProductName = product?.Name
            };
        }

        return new StockStatus
        {
            IsStock = true
        };


    }
}