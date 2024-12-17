using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permissions;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        public AccountSearchModel SearchModel;
        public SelectList Roles;
        public List<AccountViewModel> Accounts;

        private readonly IRoleApplication _roleApplication;
        private readonly IAccountApplication _accountApplication;
        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        [NeedsPermission(AccountPermissions.ListAccounts)]
        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new SelectList(_roleApplication.List(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
        }

        [NeedsPermission(AccountPermissions.CreateAccount)]
        public IActionResult OnGetCreate()
        {
            var command = new RegisterAccount()
            {
                Roles = _roleApplication.List()
            };
            return Partial("./Create", command);
        }

        [NeedsPermission(AccountPermissions.CreateAccount)]
        public IActionResult OnPostCreate(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            return new JsonResult(result);
        }

        [NeedsPermission(AccountPermissions.EditAccounts)]
        public IActionResult OnGetEdit(long id)
        {
            var account = _accountApplication.GetDetails(id);
            account.Roles = _roleApplication.List();
            return Partial("./Edit", account);
        }

        [NeedsPermission(AccountPermissions.EditAccounts)]
        public JsonResult OnPostEdit(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedsPermission(AccountPermissions.ChangePasswordAccounts)]
        public IActionResult OnGetChangePassword(long id)
        {
            var account = new ChangePassword() { Id = id };

            return Partial("./ChangePassword", account);
        }

        [NeedsPermission(AccountPermissions.ChangePasswordAccounts)]
        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }


    }
}
