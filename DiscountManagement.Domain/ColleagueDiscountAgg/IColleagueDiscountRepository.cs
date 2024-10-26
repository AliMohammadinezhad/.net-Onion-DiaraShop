using DiscountManagement.Application.Contract.ColleagueDiscount;
using Framework.Domain;

namespace DiscountManagement.Domain.ColleagueDiscountAgg;

public interface IColleagueDiscountRepository : IRepository<long, ColleagueDiscount>
{
    EditColleagueDiscount GetDetails(long id);
    List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel);
}