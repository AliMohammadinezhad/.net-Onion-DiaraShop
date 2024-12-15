using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Domain.Services;

public interface IShopInventoryAcl
{
    bool DecreaseFromInventory(List<OrderItem> orderItems);
}