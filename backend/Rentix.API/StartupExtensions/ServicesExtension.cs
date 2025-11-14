using Microsoft.AspNetCore.Identity;
using Rentix.API.Middlewares;
using Rentix.Application;
using Rentix.Domain.IdentityEntities;
using Rentix.Infrastructure;
using Rentix.Infrastructure.Persistence;

namespace Rentix.API.StartupExtensions
{
    public static class ServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            //Register MediatR
            services.AddApplicationServices();

            services.AddInfrastructure(configuration, environment);

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

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
