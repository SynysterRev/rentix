using Microsoft.EntityFrameworkCore;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Infrastructure.Persistence.Queries
{
    /// <summary>
    /// Implements query methods for retrieving Tenant data.
    /// </summary>
    public class TenantQueries : ITenantQueries
    {
        private readonly ApplicationDbContext _dbContext;

        public TenantQueries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TenantDto>> GetAllTenantsAsync()
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .Select(t => new TenantDto(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.Phone))
                .ToListAsync();
        }

        public async Task<TenantDto?> GetTenantByIdAsync(int tenantId)
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .Where(t => t.Id == tenantId)
                .Select(t => new TenantDto(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.Phone))
                .FirstOrDefaultAsync();
        }

        public async Task<TenantDto?> GetTenantByEmailAsync(string email)
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .Where(t => t.Email.Value == email)
                .Select(t => new TenantDto(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.Phone))
                .FirstOrDefaultAsync();
        }

        public async Task<List<TenantDto>> GetActiveTenantsAsync()
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .Where(t => t.Leases.Any(l => l.IsActive))
                .Select(t => new TenantDto(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.Phone))
                .ToListAsync();
        }

        public async Task<List<TenantDto>> GetTenantsByLeaseIdAsync(int leaseId)
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .Where(t => t.Leases.Any(l => l.Id == leaseId))
                .Select(t => new TenantDto(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.Phone))
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Tenants
                .AnyAsync(t => t.Email.Value == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email, int excludeTenantId)
        {
            return await _dbContext.Tenants
                .AnyAsync(t => t.Email.Value == email && t.Id != excludeTenantId);
        }
    }
}
