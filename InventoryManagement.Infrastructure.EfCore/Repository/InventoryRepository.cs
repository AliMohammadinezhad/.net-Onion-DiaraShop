using AccountManagement.Infrastructure.EfCore;
using Framework.Application;
using Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EfCore;

namespace InventoryManagement.Infrastructure.EfCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly InventoryContext _context;
    private readonly AccountContext _accountContext; 
    private readonly ApplicationDbContext _applicationDbContext;
    public InventoryRepository(InventoryContext context, ApplicationDbContext applicationDbContext, AccountContext accountContext) : base(context)
    {
        _context = context;
        _applicationDbContext = applicationDbContext;
        _accountContext = accountContext;
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
        var accounts = _accountContext.Accounts.Select(x => new { x.Id, x.FullName }).ToList(); 
        var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
        {
            Id = x.Id,
            Count = x.Count,
            CurrentCount = x.CurrentCount,
            Description = x.Description,
            InventoryId = x.InventoryId,
            Operation = x.Operation,
            OperationDate = x.OperationDate.ToFarsi(),
            OrderId = x.OrderId
        }).OrderByDescending(x => x.Id).ToList();

        foreach (var operation in operations)
        {
            operation.Operator = accounts.FirstOrDefault(x => x.Id == operation.OperatorId).FullName;
        }

        return operations;
    }
}