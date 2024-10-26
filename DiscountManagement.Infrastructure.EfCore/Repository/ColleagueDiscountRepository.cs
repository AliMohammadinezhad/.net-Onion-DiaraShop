using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;

namespace DiscountManagement.Infrastructure.EfCore.Repository;

public class ColleagueDiscountRepository : RepositoryBase<long, ColleagueDiscount>, IColleagueDiscountRepository
{
    private readonly DiscountContext _context;
    private readonly ApplicationDbContext _applicationDbContext;
    public ColleagueDiscountRepository(DiscountContext context, ApplicationDbContext applicationDbContext) : base(context)
    {
        _context = context;
        _applicationDbContext = applicationDbContext;
    }

    public EditColleagueDiscount GetDetails(long id)
    {
        return _context.ColleagueDiscounts.Select(x => new EditColleagueDiscount
        {
            DiscountRate = x.DiscountRate,
            Id = x.Id,
            ProductId = x.ProductId,
            //Products = 
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
    {
        var products = _applicationDbContext.Products.Select(x => new {x.Id, x.Name}).ToList();
        var query = _context.ColleagueDiscounts.Select(x => new ColleagueDiscountViewModel
        {
            DiscountRate = x.DiscountRate.ToString(),
            CreationDate = x.CreationDate.ToFarsi(),
            ProductId = x.ProductId,
            Id = x.Id

        });

        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        var discounts = query.OrderByDescending(x => x.Id).ToList();
        discounts.ForEach(discount =>
            discount.Product = products.FirstOrDefault(p => p.Id == discount.ProductId)?.Name);
        
        return discounts;
    }
}