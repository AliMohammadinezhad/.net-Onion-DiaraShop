using BlogManagement.Application;
using BlogManagement.Application.Contract.Article;
using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.Configuration.Permissions;
using BlogManagement.Infrastructure.EfCore;
using BlogManagement.Infrastructure.EfCore.Repository;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Query.Contracts.Article;
using Query.Contracts.ArticleCategory;
using Query.Query;

namespace BlogManagement.Infrastructure.Configuration;

public class BlogManagementDependencyInjection
{
    public static void Configuration(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IArticleCategoryApplication, ArticleCategoryApplication>();
        services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();

        services.AddTransient<IArticleApplication, ArticleApplication>();
        services.AddTransient<IArticleRepository, ArticleRepository>();

        services.AddTransient<IArticleQuery, ArticleQuery>();
        services.AddTransient<IArticleCategoryQuery, ArticleCategoryQuery>();

        services.AddTransient<IPermissionExposer, BlogPermissionExposer>();

        services.AddDbContext<BlogContext>(x => x.UseSqlServer(connectionString));
    }
    
}