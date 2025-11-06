using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rentix.Domain.Entities;
using Rentix.Domain.IdentityEntities;
using Rentix.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

namespace Rentix.Tests.Integration.Setup
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.UseEnvironment("Production");

            builder.ConfigureTestServices(services =>
            {
                // Remove existing DbContext registrations
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register DbContext with PostgreSQL test container
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                // Ensure database is created and seeded
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();

                Seed(dbContext);
            });
        }

        public readonly int DefaultAddressId = 1;
        public readonly Guid DefaultUserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");


        private void Seed(ApplicationDbContext context)
        {
            if (!context.Addresses.Any(a => a.Id == DefaultAddressId))
            {
                var address = Address.Create(
                    "456 Main St",
                    "12345",
                    "Test City",
                    "Test Country",
                    "Apt 4B"
                );
                address.Id = DefaultAddressId;
                context.Addresses.Add(address);
            }

            if (!context.Users.Any(u => u.Id == DefaultUserId))
            {
                var user = new ApplicationUser
                {
                    Id = DefaultUserId,
                    UserName = "test.user",
                    Email = "test.user@test.com",
                    PhoneNumber = "+33123456789",
                    AddressId = DefaultAddressId
                };

                context.Users.Add(user);
            }

            context.SaveChanges();
            context.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Addresses\"', 'Id'), COALESCE(MAX(\"Id\"), 1)) FROM \"Addresses\";");
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
        public async Task ResetDatabaseAsync()
        {
            // Obtient un nouveau scope de service
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Supprime la base de données existante (si elle existe)
            await dbContext.Database.EnsureDeletedAsync();

            // 2. Recrée la base de données et applique le schéma
            // Si vous utilisez des migrations, remplacez EnsureCreatedAsync() par MigrateAsync()
            await dbContext.Database.EnsureCreatedAsync();
            // ou await dbContext.Database.MigrateAsync(); si vous utilisez des migrations

            // 3. Réensemencer les données de base (Seed)
            Seed(dbContext);
        }
    }
}
