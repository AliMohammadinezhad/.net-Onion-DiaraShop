using AccountManagement.Application.Contract.Account;
using Framework.Domain;

namespace AccountManagement.Domain.AccountAgg;

public interface IAccountRepository : IRepository<long, Account>
{
    EditAccount GetDetails(long id);
    List<AccountViewModel> Search(AccountSearchModel searchModel);
}