using Framework.Domain;
using ShopManagement.Contracts.Order;

namespace ShopManagement.Domain.OrderAgg;

public interface IOrderRepository : IRepository<long, Order>
{
    double GetAmountBy(long id);
    List<OrderViewModel> GetTotalPlacedOrders();
    List<OrderViewModel> GetTotalOrders();
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}