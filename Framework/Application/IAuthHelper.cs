namespace Framework.Application;

public interface IAuthHelper
{
    bool IsAuthenticated();
    void SignIn(AuthViewModel account);
    void SignOut();

}