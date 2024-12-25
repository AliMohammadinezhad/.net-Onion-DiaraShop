using Framework.Application;
using Framework.Application.Sms;
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
    private readonly ISmsService _smsService;
    private readonly IShopAccountAcl _shopAccountAcl;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl, ISmsService smsService, IShopAccountAcl shopAccountAcl)
    {
        _orderRepository = orderRepository;
        _authHelper = authHelper;
        _configuration = configuration;
        _shopInventoryAcl = shopInventoryAcl;
        _smsService = smsService;
        _shopAccountAcl = shopAccountAcl;
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
        if (!_shopInventoryAcl.DecreaseFromInventory(order.Items)) return "";

        _orderRepository.SaveChanges();
        var account = _shopAccountAcl.GetAccountById(order.AccountId);

        _smsService.Send(account.mobile, $"{account.name} گرامی سفارش شما با شماره پیگیری {issueTrackingNumber} .موفقیت پرداخت شد");
        return issueTrackingNumber;

    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }

    public void Cancel(long id)
    {
        var order = _orderRepository.Get(id);
        order.Cancel();
        _orderRepository.SaveChanges();
    }

    public List<OrderViewModel> GetTotalPlacedOrders()
    {
        return _orderRepository.GetTotalPlacedOrders();
    }

    public List<OrderViewModel> GetTotalOrders()
    {
        return _orderRepository.GetTotalOrders();
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        return _orderRepository.GetItems(orderId);
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _orderRepository.Search(searchModel);
    }
}