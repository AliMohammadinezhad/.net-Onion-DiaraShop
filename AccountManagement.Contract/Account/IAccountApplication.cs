using Framework.Application;

namespace AccountManagement.Application.Contract.Account;

public interface IAccountApplication
{
    AccountViewModel GetAccountBy(long id);
    OperationResult Register(RegisterAccount command);
    OperationResult Edit(EditAccount command);
    OperationResult ChangePassword(ChangePassword command);
    OperationResult Login(Login command);
    EditAccount GetDetails(long id);
    List<AccountViewModel> GetAccounts();
    List<AccountViewModel> Search(AccountSearchModel searchModel);
    void Logout();
}