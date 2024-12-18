using AccountManagement.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Application.Contract.Visitor;
using AccountManagement.Application.Contract.VisitorService;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Domain.VisitorAgg;
using AccountManagement.Infrastructure.Configuration.Permissions;
using AccountManagement.Infrastructure.EfCore;
using AccountManagement.Infrastructure.EfCore.Repository;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure.Configuration
{
    public class AccountManagementDependencyInjection
    {
        public static void Configuration(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IAccountApplication, AccountApplication>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            services.AddTransient<IRoleApplication, RoleApplication>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            services.AddTransient<IVisitorRepository, VisitorRepository>();
            services.AddTransient<IVisitorApplication, VisitorApplication>();
            services.AddTransient<IVisitorService, VisitorService>();

            services.AddTransient<IPermissionExposer, AccountPermissionExposer>();

            services.AddDbContext<AccountContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
