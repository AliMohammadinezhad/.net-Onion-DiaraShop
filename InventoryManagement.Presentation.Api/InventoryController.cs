using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc;
using Query.Contracts.Inventory;

namespace InventoryManagement.Presentation.Api;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryQuery _inventoryQuery;
    private readonly IInventoryApplication _inventoryApplication;
    public InventoryController(IInventoryQuery inventoryQuery, IInventoryApplication inventoryApplication)
    {
        _inventoryQuery = inventoryQuery;
        _inventoryApplication = inventoryApplication;
    }

    [HttpGet("{id:long}")]
    public List<InventoryOperationViewModel> GetOperationBy(long id)
    {
        return _inventoryApplication.GetOperationLog(id);
    }

    [HttpPost]
    public StockStatus CheckStock([FromBody] IsInStock command)
    {
        return _inventoryQuery.CheckStock(command);
    } 
}