using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<DaskataDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<UserProfile, UserRole>(options => options.User.RequireUniqueEmail = true)
                .AddRoles<UserRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DaskataDbContext>();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //.AddEntityFrameworkStores<DaskataDbContext>();

            return services;
        }
    }
}
