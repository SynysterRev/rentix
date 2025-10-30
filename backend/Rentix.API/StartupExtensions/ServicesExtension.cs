using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rentix.Application;
using Rentix.Domain.IdentityEntities;
using Rentix.Infrastructure.DatabaseContext;
using System.Reflection;

namespace Rentix.API.StartupExtensions
{
    public static class ServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                if (!string.IsNullOrEmpty(databaseUrl))
                {
                    var uri = new Uri(databaseUrl);
                    var userInfo = uri.UserInfo.Split(':');

                    var connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

                    configuration["ConnectionStrings:Default"] = connectionString;
                }
            }

            //Register MediatR
            services.AddApplicationServices();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            var originsString = configuration.GetValue<string>("AllowedOrigins");
            var defaultOrigins = new string[] { "http://localhost:4200" };
            var allowedOrigins = originsString?
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToArray()
                            ?? defaultOrigins;

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .AllowCredentials();
                });
            });
        }
    }
}
