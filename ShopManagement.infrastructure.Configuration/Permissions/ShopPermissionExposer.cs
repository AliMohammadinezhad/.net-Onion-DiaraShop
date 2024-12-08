using Framework.Infrastructure;
using static ShopManagement.infrastructure.Configuration.Permissions.ShopPermissions;

namespace ShopManagement.infrastructure.Configuration.Permissions;

public class ShopPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDTO>> Expose()
    {
        return new Dictionary<string, List<PermissionDTO>>
        {
            {
                "Products", [
                    new PermissionDTO(ListProducts, "لیست محصولات"),
                    new PermissionDTO(SearchProducts, "جست و جوی محصولات"),
                    new PermissionDTO(CreateProduct, "ایجاد محصول"),
                    new PermissionDTO(EditProducts, "ویرایش محصول")
                ]
            },
            {
                "Product Categories", [
                    new PermissionDTO(ListProductCategories, "لیست گروه محصول"),
                    new PermissionDTO(SearchProductCategories, "جست و جوی گروه محصول"),
                    new PermissionDTO(CreateProductCategories, "ایجاد گروه محصول"),
                    new PermissionDTO(EditProductCategories, "ویرایش گروه محصول")
                ]
            },
            {
                "Product Pictures", [
                    new PermissionDTO(ListProductPictures, "لیست عکس محصول"),
                    new PermissionDTO(SearchProductPictures, "جست و جوی عکس محصول"),
                    new PermissionDTO(CreateProductPictures, "ایجاد عکس محصول"),
                    new PermissionDTO(EditProductPictures, "ویرایش عکس محصول")
                ]
            },
            {
                "Slides", [
                    new PermissionDTO(ListSlides, "لیست اسلاید"),
                    new PermissionDTO(CreateSlides, "ایجاد اسلاید"),
                    new PermissionDTO(EditSlides, "ویرایش اسلاید"),
                    new PermissionDTO(RemoveSlides, "حذف اسلاید"),
                    new PermissionDTO(RestoreSlides, "بازیابی اسلاید"),
                ]
            }
        };
    }
}