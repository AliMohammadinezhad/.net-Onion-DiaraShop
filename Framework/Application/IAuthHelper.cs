namespace Framework.Application;

public interface IAuthHelper
{
    bool IsAuthenticated();
    void SignOut();
    void SignIn(AuthViewModel account);
    string CurrentAccountRole();
    AuthViewModel CurrentAccountInfo();
    List<int> GetPermissions();
    long CurrentAccountId();
}