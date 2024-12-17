using AccountManagement.Infrastructure.EfCore;
using Framework.Application;
using Framework.Infrastructure;
using ShopManagement.Contracts;
using ShopManagement.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EfCore.Repository;

public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly AccountContext _accountContext;
    public OrderRepository(ApplicationDbContext context, AccountContext accountContext) : base(context)
    {
        _context = context;
        _accountContext = accountContext;
    }


    public double GetAmountBy(long id)
    {
        var order = _context.Orders
            .Select(x => new { x.PayAmount, x.Id })
            .FirstOrDefault(x => x.Id == id);
        if (order != null)
            return order.PayAmount;
        return 0;
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        var products = _context.Products.Select(x => new { x.Id, x.Name }).ToList();
        var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
        if (order == null)
            return [];
        var items = order.Items.Select(x => new OrderItemViewModel
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            Count = x.Count,
            OrderId = x.OrderId,
            UnitPrice = x.UnitPrice,
            ProductId = x.ProductId
        }).ToList();


        foreach (var item in items)
        {
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
        }

        return items;
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        var accounts = _accountContext.Accounts.Select(x => new { x.Id, x.FullName }).ToList();
        var query = _context.Orders.Select(x => new OrderViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            DiscountAmount = x.DiscountAmount,
            CreationDate = x.CreationDate.ToFarsi(),
            IsCancelled = x.IsCancelled,
            IsPaid = x.IsPaid,
            IssueTrackingNumber = x.IssueTrackingNumber,
            PayAmount = x.PayAmount,
            PaymentMethodId = x.PaymentMethod,
            RefId = x.RefId,
            TotalPrice = x.TotalPrice,
        });

        query = query.Where(x => x.IsCancelled == searchModel.IsCancelled);

        if (searchModel.AccountId > 0)
            query = query.Where(x => x.AccountId == searchModel.AccountId);


        var orders = query.OrderByDescending(x => x.Id).ToList();


        foreach (var order in orders)
        {
            order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.FullName;
            order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId)?.Name;
        }

        return orders;
    }
}