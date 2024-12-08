using AccountManagement.Application.Contract.Role;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class EditModel : PageModel
    {
        public EditRole Command { get; set; }
        public List<SelectListItem> Permissions { get; set; } = [];

        private readonly IEnumerable<IPermissionExposer> _exposers;
        private readonly IRoleApplication _roleApplication;
        public EditModel(IRoleApplication roleApplication,
            IEnumerable<IPermissionExposer> exposers)
        {
            _roleApplication = roleApplication;
            _exposers = exposers;
        }

        public void OnGet(long roleCreateId)
        {
            Command = _roleApplication.GetDetails(roleCreateId);
            foreach (var exposer in _exposers)
            {
                var exposedPermission = exposer.Expose();
                foreach (var (key, value) in exposedPermission)
                {
                    var group = new SelectListGroup
                    {
                        Name = key
                    };
                    foreach (var permission in value)
                    {
                        var item = new SelectListItem(permission.Name, permission.Code.ToString())
                        {
                            Group = group
                        };
                        if(Command.MappedPermission.Any(x => x.Code == permission.Code))
                            item.Selected = true;

                        Permissions.Add(item);
                    }
                }
            }
        }

        public IActionResult OnPost(EditRole command)
        {
            _roleApplication.Edit(command);
            return RedirectToPage("./Index");
        }
    }
}
