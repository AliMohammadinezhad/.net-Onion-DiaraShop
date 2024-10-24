using DiscountManagement.Application.Contract.CustomerDiscount;
using Framework.Domain;

namespace DiscountManagement.Domain.CustomerDiscountAgg;

public interface ICustomerDiscountRepository : IRepository<long, CustomerDiscount>
{
    EditCustomerDiscount GetDetails(long id);
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
}