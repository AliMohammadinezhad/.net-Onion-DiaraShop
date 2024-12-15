using Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IOrderRepository _orderRepository;
    private readonly IConfiguration _configuration;
    private readonly IAuthHelper _authHelper;
    private readonly IShopInventoryAcl _shopInventoryAcl;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl)
    {
        _orderRepository = orderRepository;
        _authHelper = authHelper;
        _configuration = configuration;
        _shopInventoryAcl = shopInventoryAcl;
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();
        var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount);

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
        if (_shopInventoryAcl.DecreaseFromInventory(order.Items))
        {
            _orderRepository.SaveChanges();
            return issueTrackingNumber;
        }

        return null;
    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }
}