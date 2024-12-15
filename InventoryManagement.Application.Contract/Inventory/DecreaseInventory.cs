using Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Application.Contract.Inventory;

public class DecreaseInventory
{
    public long InventoryId { get; set; }
    public long ProductId { get; set; }
    [Range(0, 100000000, ErrorMessage = ValidationMessages.IsPositive)]
    public long Count { get; set; }
    public string Description { get; set; }
    public long OrderId { get; set; }

    public DecreaseInventory()
    {
        
    }

    public DecreaseInventory(
        long productId,
        long count,
        string description,
        long orderId)
    {
        ProductId = productId;
        Count = count;
        Description = description;
        OrderId = orderId;
    }
}