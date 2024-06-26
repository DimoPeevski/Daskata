﻿using Daskata.Core.Contracts.Answer;
using Daskata.Core.Contracts.Exam;
using Daskata.Core.Contracts.Network;
using Daskata.Core.Contracts.Profile;
using Daskata.Core.Contracts.Question;
using Daskata.Core.Contracts.User;
using Daskata.Core.Services.Answer;
using Daskata.Core.Services.Error;
using Daskata.Core.Services.Exam;
using Daskata.Core.Services.Network;
using Daskata.Core.Services.Profile;
using Daskata.Core.Services.Question;
using Daskata.Core.Services.User;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Daskata.Infrastructure.Data.SeedDb;
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
            services.AddScoped<INetworkService, NetworkService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
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

            services.AddScoped<SeedData>();

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

        public static async Task RolesSeedAsync(this IServiceCollection services, IConfiguration config)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
                await SeedRolesAsync(roleManager);
            }
        }

        public static async Task InitializeApplicationData(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();

                await seeder.InitializeDbSeed();
            }
        }

        private static async Task SeedRolesAsync(RoleManager<UserRole> roleManager)
        {
            var roles = new[] { "Admin", "Manager", "Teacher", "Student" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new UserRole 
                    { 
                        Name = roleName 
                    });
                }
            }
        }
    }
}
