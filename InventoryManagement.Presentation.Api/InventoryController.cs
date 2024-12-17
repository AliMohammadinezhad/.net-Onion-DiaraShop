using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Presentation.Api;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryApplication _inventoryApplication;

    public InventoryController(IInventoryApplication inventoryApplication)
    {
        _inventoryApplication = inventoryApplication;
    }

    [HttpGet("{id:long}")]
    public List<InventoryOperationViewModel> GetOperationBy(long id)
    {
        return _inventoryApplication.GetOperationLog(id);
    }
}