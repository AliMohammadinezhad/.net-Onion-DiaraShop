using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EfCore.Repository;

public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
{
    private readonly AccountContext _context;
    public AccountRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public Account GetByUserName(string userName)
    {
        return _context.Accounts.FirstOrDefault(x => x.Username == userName);
    }

    public EditAccount GetDetails(long id)
    {
        return _context.Accounts.Select(x => new EditAccount()
        {
            Id = x.Id,
            FullName = x.FullName,
            Mobile = x.Mobile,
            Password = x.Password,
            RoleId = x.RoleId,
            Username = x.Username
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<AccountViewModel> GetAccounts()
    {
        return _context.Accounts.Select(x => new AccountViewModel
        {
            Id = x.Id,
            FullName = x.FullName
        }).ToList();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        var query = _context.Accounts
            .Include(x => x.Role)
            .Select(x => new AccountViewModel()
            {
                Id = x.Id,
                Username = x.Username,
                FullName = x.FullName,
                Mobile = x.Mobile,
                ProfilePhoto = x.ProfilePhoto,
                Role = x.Role.Name,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi(),
                CreationDateCalculation = x.CreationDate
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Username))
            query = query.Where(x => x.Username.Contains(searchModel.Username));

        if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            query = query.Where(x => x.FullName.Contains(searchModel.FullName));

        if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
            query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));

        if (searchModel.RoleId > 0)
            query = query.Where(x => x.RoleId.Equals(searchModel.RoleId));

        return query.OrderByDescending(x => x.Id).ToList();
    }
}