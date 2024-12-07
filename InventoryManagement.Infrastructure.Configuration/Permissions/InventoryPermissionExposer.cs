using Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permissions;

public class InventoryPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
                "Inventory", [
                    new PermissionDTO(50, "ListInventory"),
                    new PermissionDTO(51, "SearchInventory"),
                    new PermissionDTO(52, "CreateInventory"),
                    new PermissionDTO(53, "EditInventory")
                ]
            }
        };
    }
}