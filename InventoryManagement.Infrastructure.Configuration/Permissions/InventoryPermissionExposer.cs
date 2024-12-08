using Framework.Infrastructure;
using static InventoryManagement.Infrastructure.Configuration.Permissions.InventoryPermissions;
namespace InventoryManagement.Infrastructure.Configuration.Permissions;

public class InventoryPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
            "Inventory", [
                new PermissionDTO(ListInventory, "لیست انبار"),
                new PermissionDTO(SearchInventory, "جست و جوی انبار"),
                new PermissionDTO(CreateInventory, "ایجاد انبار"),
                new PermissionDTO(EditInventory, "ویرایش انبار"),
                new PermissionDTO(IncreaseInventory, "افزایش موجودی انبار"),
                new PermissionDTO(DecreaseInventory, "کاهش موجودی انبار"),
                new PermissionDTO(OperationLogInventory, "گزارش انبار"),
            ]
        }
        };
    }
}