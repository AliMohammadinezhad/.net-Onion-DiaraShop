using Framework.Infrastructure;

namespace ShopManagement.infrastructure.Configuration.Permissions;

public class ShopPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
                "Product", new List<PermissionDTO>
                {
                    new PermissionDTO(10, "ListProducts"),
                    new PermissionDTO(11, "SearchProducts"),
                    new PermissionDTO(12, "CreateProducts"),
                    new PermissionDTO(13, "EditProducts"),
                }
            },
            {
                "ProductCategory", new List<PermissionDTO>
                {
                    new PermissionDTO(20, "ListProductCategories"),
                    new PermissionDTO(21, "SearchProductCategories"),
                    new PermissionDTO(22, "CreateProductCategories"),
                    new PermissionDTO(23, "EditProductCategories"),
                }
            }
        };
    }
}