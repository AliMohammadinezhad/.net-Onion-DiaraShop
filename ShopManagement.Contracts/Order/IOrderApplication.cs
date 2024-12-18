namespace ShopManagement.Contracts.Order;

public interface IOrderApplication
{
    long PlaceOrder(Cart cart);
    string PaymentSucceeded(long orderId, long refId);
    double GetAmountBy(long id);
    void Cancel(long id);
    List<OrderViewModel> GetTotalPlacedOrders();
    List<OrderViewModel> GetTotalOrders();
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}