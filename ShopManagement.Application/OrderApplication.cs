using Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IOrderRepository _orderRepository;
    private readonly IConfiguration _configuration;
    private readonly IAuthHelper _authHelper;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration)
    {
        _orderRepository = orderRepository;
        _authHelper = authHelper;
        _configuration = configuration;
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();
        var order = new Order(currentAccountId, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount);

        foreach (var item in cart.CartItems)
        {
            var orderItem = new OrderItem(item.Id, item.Count, item.UnitPrice, item.DiscountRate);
            order.AddItem(orderItem);
        }

        _orderRepository.Create(order);
        _orderRepository.SaveChanges();
        return order.Id;
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _orderRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        var symbol = _configuration.GetSection("Symbol").Value;
        var issueTrackingNumber = CodeGenerator.Generate(symbol);
        order.SetIssueTrackingNumber(issueTrackingNumber);
        // Reduce OrderItems from Inventory
        _orderRepository.SaveChanges();
        return issueTrackingNumber;
    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }
}