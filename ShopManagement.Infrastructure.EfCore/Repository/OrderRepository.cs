using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EfCore.Repository;

public class OrderRepository : RepositoryBase<long,Order>, IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public double GetAmountBy(long id)
    {
        var order = _context.Orders
            .Select(x => new { x.PayAmount, x.Id })
            .FirstOrDefault(x => x.Id == id);
        if(order != null)
            return order.PayAmount;
        return 0;
    }
}