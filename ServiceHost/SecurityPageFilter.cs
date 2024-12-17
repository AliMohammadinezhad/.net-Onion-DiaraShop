using Framework.Application;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ServiceHost;

public class SecurityPageFilter : IPageFilter
{
    private readonly IAuthHelper _authHelper;

    public SecurityPageFilter(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {

    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var handlerPermission = (NeedsPermissionAttribute)context?
            .HandlerMethod?
            .MethodInfo.GetCustomAttribute(typeof(NeedsPermissionAttribute))!;

        if (handlerPermission == null) return;

        var accountPermissions = _authHelper.GetPermissions();

        if (accountPermissions.All(x => x != handlerPermission.Permissions))
            context?.HttpContext?.Response?.Redirect("/Account");

    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
    }
}