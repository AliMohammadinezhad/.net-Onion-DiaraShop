using Framework.Infrastructure;
using static AccountManagement.Infrastructure.Configuration.Permissions.AccountPermissions;
namespace AccountManagement.Infrastructure.Configuration.Permissions;

public class AccountPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
            "Accounts", [
                new PermissionDTO(ListAccounts, "لیست یوزر ها"),
                new PermissionDTO(SearchAccounts, "جست و جوی یوزر ها"),
                new PermissionDTO(CreateAccount, "ایجاد یوزر"),
                new PermissionDTO(EditAccounts, "ویرایش یوزر ها"),
                new PermissionDTO(ChangePasswordAccounts, "تغییر رمز عبور یوزر ها"),
            ]
            }
        };
    }
}