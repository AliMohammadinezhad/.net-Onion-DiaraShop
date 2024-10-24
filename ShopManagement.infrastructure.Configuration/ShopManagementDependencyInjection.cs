using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Query.Contracts.ProductCategory;
using Query.Contracts.Slide;
using Query.Query;
using ShopManagement.Application;
using ShopManagement.Contracts.Product;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.Contracts.ProductPicture;
using ShopManagement.Contracts.Slide;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.EfCore;
using ShopManagement.Infrastructure.EfCore.Repository;

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

        services.AddTransient<ISlideQuery, SlideQuery>();
        services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    }
}