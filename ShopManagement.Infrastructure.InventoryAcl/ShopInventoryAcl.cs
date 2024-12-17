using InventoryManagement.Application.Contract.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public bool DecreaseFromInventory(List<OrderItem> orderItems)
        {
            var command = orderItems
                .Select(
                    orderItem => new DecreaseInventory(
                        orderItem.ProductId,
                        orderItem.Count,
                        "خرید مشتری",
                        orderItem.OrderId)
                    ).ToList();

            return _inventoryApplication.Decrease(command).IsSucceeded;
        }
    }
}
