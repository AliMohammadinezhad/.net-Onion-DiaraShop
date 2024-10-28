using Framework.Application;
using Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EfCore;

namespace InventoryManagement.Infrastructure.EfCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly InventoryContext _context;
    private readonly ApplicationDbContext _applicationDbContext;
    public InventoryRepository(InventoryContext context, ApplicationDbContext applicationDbContext) : base(context)
    {
        _context = context;
        _applicationDbContext = applicationDbContext;
    }

    public EditInventory GetDetails(long id)
    {
        return _context.Inventory.Select(x => new EditInventory
        {
            Id = x.Id,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).FirstOrDefault(x => x.Id == id);
    }

    public Inventory GetBy(long productId)
    {
        return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        var products = _applicationDbContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        var query = _context.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (searchModel.InStock)
            query = query.Where(x => !x.InStock);

        var inventory = query.OrderByDescending(x => x.Id).ToList();
        inventory.ForEach(item =>
        {
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
        });


        return inventory;

    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        var inventory = _context.Inventory.FirstOrDefault(x => x.Id == inventoryId);
        return inventory.Operations.Select(x => new InventoryOperationViewModel
        {
            Id = x.Id,
            Count = x.Count,
            CurrentCount = x.CurrentCount,
            Description = x.Description,
            InventoryId = x.InventoryId,
            Operation = x.Operation,
            OperationDate = x.OperationDate.ToFarsi(),
            Operator = "مدیر سیستم",
            OperatorId = x.OperatorId,
            OrderId = x.OrderId
        }).OrderByDescending(x => x.Id).ToList();
    }
}