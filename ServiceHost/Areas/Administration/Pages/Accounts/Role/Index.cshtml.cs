using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role;

public class IndexModel : PageModel
{
    public List<RoleViewModel> Roles;

    private readonly IRoleApplication _roleApplication;
    public IndexModel(IRoleApplication roleApplication)
    {
        _roleApplication = roleApplication;
    }

    public void OnGet()
    {
        Roles = _roleApplication.List();
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateRole();
        return Partial("./Create", command);
    }

    public IActionResult OnPostCreate(CreateRole command)
    {
        var result = _roleApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var account = _roleApplication.GetDetails(id);
        return Partial("./Edit", account);
    }

    public JsonResult OnPostEdit(EditRole command)
    {
        var result = _roleApplication.Edit(command);
        return new JsonResult(result);
    }


}