using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permissions;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class CreateModel : PageModel
    {
        public CreateRole Command { get; set; }
        private readonly IRoleApplication _roleApplication;

        public CreateModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        [NeedsPermission(AccountPermissions.CreateRole)]
        public void OnGet()
        {
        }

        [NeedsPermission(AccountPermissions.CreateRole)]
        public IActionResult OnPost(CreateRole command)
        {
            var result = _roleApplication.Create(command);
            return RedirectToPage("./Index");
        }
    }
}
