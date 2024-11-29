using CommentManagement.Application;
using CommentManagement.Contract.Comment;
using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.EfCore;
using CommentManagement.Infrastructure.EfCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommentManagement.Infrastructure.Configuration
{
    public class CommentManagementDependencyInjection
    {
        public static void Configuration(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentApplication, CommentApplication>();


            services.AddDbContext<CommentContext>(option => option.UseSqlServer(connectionString));
        }
    }
}
