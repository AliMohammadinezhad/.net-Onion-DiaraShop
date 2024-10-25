using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;

namespace DiscountManagement.Infrastructure.EfCore.Repository;

public class CustomerDiscountRepository : RepositoryBase<long, CustomerDiscount>, ICustomerDiscountRepository
{
    private readonly DiscountContext _context;
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerDiscountRepository(DiscountContext context, ApplicationDbContext applicationDbContext) :
        base(context)
    {
        _context = context;
        _applicationDbContext = applicationDbContext;
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            StartDate = x.StartDate.ToFarsi(),
            EndDate = x.EndDate.ToFarsi(),
            ProductId = x.ProductId,
            Reason = x.Reason
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
    {
        var products = _applicationDbContext.Products.Select(x => new { x.Name, x.Id }).ToList();
        var query = _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            EndDate = x.EndDate.ToFarsi(),
            StartDate = x.StartDate.ToFarsi(),
            EndDateGr = x.EndDate,
            StartDateGr = x.StartDate,
            ProductId = x.ProductId,
            Reason = x.Reason,
        });

        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (!string.IsNullOrWhiteSpace(searchModel.StartDate))

            query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());
        


        if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
            query = query.Where(x => x.EndDateGr < searchModel.EndDate.ToGeorgianDateTime());

        var discounts = query.OrderByDescending(x => x.Id).ToList();
        discounts.ForEach(discount =>
        {
            discount.Product = products.FirstOrDefault(x => x.Id == discount.ProductId)?.Name;
        });
        return discounts;
    }
}