using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Moq;
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

        public string TestFilesPath { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            TestFilesPath = Path.Combine(Path.GetTempPath(), $"IntegrationTests_{Guid.NewGuid()}");
            Directory.CreateDirectory(TestFilesPath);

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

                services.AddSingleton<IWebHostEnvironment>(sp =>
                {
                    var env = new Mock<IWebHostEnvironment>();
                    env.Setup(e => e.ContentRootPath).Returns(TestFilesPath);
                    env.Setup(e => e.EnvironmentName).Returns("Test");

                    var fileProvider = new PhysicalFileProvider(TestFilesPath);
                    env.Setup(e => e.ContentRootFileProvider).Returns(fileProvider);
                    env.Setup(e => e.WebRootPath).Returns(TestFilesPath);
                    env.Setup(e => e.WebRootFileProvider).Returns(fileProvider);

                    return env.Object;
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
            if (Directory.Exists(TestFilesPath))
            {
                try
                {
                    Directory.Delete(TestFilesPath, recursive: true);
                }
                catch
                {
                    // Ignore cleanup errors in tests
                }
            }
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
        public async Task ResetDatabaseAsync()
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();

            await dbContext.Database.EnsureCreatedAsync();

            Seed(dbContext);
        }
    }
}
