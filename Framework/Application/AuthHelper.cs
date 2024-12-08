using System.Security.Claims;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Framework.Application;

public class AuthHelper : IAuthHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool IsAuthenticated()
    {
        return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        //var claims = _contextAccessor.HttpContext.User.Claims.ToList();
        //return claims.Count > 0;
    }

    public void SignIn(AuthViewModel account)
    {
        var permissions = JsonConvert.SerializeObject(account.Permissions);
        var claims = new List<Claim>
        {
            new("AccountId", account.AccountId.ToString()),
            new(ClaimTypes.Name, account.FullName),
            new(ClaimTypes.Role, account.RoleId.ToString()),
            new("Username", account.Username),
            new("Permissions", permissions)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
        };

        _contextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

    }

    public string CurrentAccountRole()
    {
        return IsAuthenticated() ? _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value : null;
    }

    public AuthViewModel CurrentAccountInfo()
    {
        var result = new AuthViewModel();
        if (!IsAuthenticated())
            return result;

        var claims = _contextAccessor.HttpContext.User.Claims.ToList();
        result.AccountId = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId")?.Value!);
        result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!);
        result.Username = claims.FirstOrDefault(x => x.Type == "Username")?.Value!;
        result.FullName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value!;
        result.Role = Roles.GetRoleBy(result.RoleId);
        return result;
    }

    public List<int> GetPermissions()
    {
        if (!IsAuthenticated())
            return new List<int>();

        var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Permissions")?.Value;

        if (permissions != null)
            return JsonConvert.DeserializeObject<List<int>>(permissions)
                   ??
                   throw new JsonException("Deserializing Failed");
        throw new NullReferenceException("permissions was null");
    }

    public void SignOut()
    {
        _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}