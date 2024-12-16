namespace ShopManagement.infrastructure.Configuration.Permissions;

public static class ShopPermissions
{
    // Orders
    public const int ListOrders = 1;
    public const int SearchOrders = 2;
    public const int ConfirmOrder = 3;
    public const int CancelOrder = 4;
    public const int ItemsOrder = 5;

    // Products
    public const int ListProducts = 10;
    public const int SearchProducts = 11;
    public const int CreateProduct = 12;
    public const int EditProducts = 13;

    // ProductCategories
    public const int ListProductCategories = 20;
    public const int SearchProductCategories = 21;
    public const int CreateProductCategories = 22;
    public const int EditProductCategories = 23;

    // ProductPictures
    public const int ListProductPictures = 30;
    public const int SearchProductPictures = 31;
    public const int CreateProductPictures = 32;
    public const int EditProductPictures = 33;

    // Slides
    public const int ListSlides = 40;
    public const int CreateSlides = 42;
    public const int EditSlides = 43;
    public const int RemoveSlides = 44;
    public const int RestoreSlides = 45;

}