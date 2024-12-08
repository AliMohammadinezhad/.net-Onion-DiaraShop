using Framework.Infrastructure;
using static DiscountManagement.infrastructure.Configuration.Permissions.DiscountPermissions;
namespace DiscountManagement.infrastructure.Configuration.Permissions;

public class DiscountPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
            "Customer Discount", [
                new PermissionDTO(ListCustomerDiscount, "لیست تخفیفات مشتری"),
                new PermissionDTO(SearchCustomerDiscount, "جست و جوی تخفیفات مشتری"),
                new PermissionDTO(CreateCustomerDiscount, "ایجاد تخفیف مشتری"),
                new PermissionDTO(EditCustomerDiscount, "ویرایش تخفیف مشتری")
            ]
        },

                {
                "Colleague Discount", [
                    new PermissionDTO(ListColleagueDiscount, "لیست تخفیفات همکاری"),
                    new PermissionDTO(SearchColleagueDiscount, "جست و جوی تخفیفات همکاری"),
                    new PermissionDTO(CreateColleagueDiscount, "ایجاد تخفیف همکاری"),
                    new PermissionDTO(EditColleagueDiscount, "ویرایش تخفیف همکاری"),
                    new PermissionDTO(RestoreColleagueDiscount, "فعال سازی تخفیف همکاری"),
                    new PermissionDTO(RemoveColleagueDiscount, "غیر فعال سازی تخفیف همکاری"),
                ]
            }
        };
    }
}