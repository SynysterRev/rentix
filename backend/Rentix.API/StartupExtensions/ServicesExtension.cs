using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

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
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseNpgsql(configuration.GetConnectionString("Default"));
            //});

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
