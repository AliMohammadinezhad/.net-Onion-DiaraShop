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


}