using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Query.Contracts;
using Query.Contracts.Inventory;
using Query.Contracts.Product;
using Query.Contracts.ProductCategory;
using Query.Contracts.Slide;
using Query.Query;
using ShopManagement.Application;
using ShopManagement.Contracts.Order;
using ShopManagement.Contracts.Product;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.Contracts.ProductPicture;
using ShopManagement.Contracts.Slide;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.Services;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.AccountAcl;
using ShopManagement.infrastructure.Configuration.Permissions;
using ShopManagement.Infrastructure.EfCore;
using ShopManagement.Infrastructure.EfCore.Repository;
using ShopManagement.Infrastructure.InventoryAcl;

namespace ShopManagement.infrastructure.Configuration;

public class ShopManagementDependencyInjection
{
    public static void Configuration(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
        services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();

        services.AddTransient<IProductApplication, ProductApplication>();
        services.AddTransient<IProductRepository, ProductRepository>();

        services.AddTransient<IProductPictureApplication, ProductPictureApplication>();
        services.AddTransient<IProductPictureRepository, ProductPictureRepository>();

        services.AddTransient<ISlideApplication, SlideApplication>();
        services.AddTransient<ISlideRepository, SlideRepository>();

        services.AddTransient<IOrderApplication, OrderApplication>();
        services.AddTransient<IOrderRepository, OrderRepository>();

        services.AddTransient<ISlideQuery, SlideQuery>();
        services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();
        services.AddTransient<IProductQuery, ProductQuery>();

        services.AddTransient<ICartCalculatorService, CartCalculatorService>();
        
        services.AddTransient<IShopInventoryAcl, ShopInventoryAcl>();
        services.AddTransient<IShopAccountAcl, ShopAccountAcl>();

        services.AddSingleton<ICartService, CartService>();

        services.AddTransient<IPermissionExposer, ShopPermissionExposer>();

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    }
}