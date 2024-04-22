using Daskata.Core.Contracts.Error;
using Daskata.Core.Contracts.Profile;
using Daskata.Core.Contracts.User;
using Daskata.Core.Services.Error;
using Daskata.Core.Services.Profile;
using Daskata.Core.Services.User;
using Daskata.Infrastructure.Common;
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IErrorService, ErrorService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<DaskataDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IRepository, Repository>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddDefaultIdentity<UserProfile>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
                .AddRoles<UserRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DaskataDbContext>();

            return services;
        }

 
        public static async Task<IServiceCollection> RolesSeedAsync(this IServiceCollection services, IConfiguration config)
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

                await SeedRolesAsync(roleManager);
            }

            return services;
        }

        private static async Task SeedRolesAsync(RoleManager<UserRole> roleManager)
        {
            var roles = new[] { "Admin", "Manager", "Teacher", "Student" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new UserRole { Name = roleName });
                }
            }
        }
    }
}
