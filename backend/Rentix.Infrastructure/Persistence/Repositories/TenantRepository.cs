using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TenantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            await _dbContext.Tenants.AddAsync(tenant);
            return tenant;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await _dbContext.Tenants.FindAsync(id);
            
            if (tenant == null)
            {
                return false;
            }

            _dbContext.Tenants.Remove(tenant);
            return true;
        }

        public async Task<Tenant?> GetTenantByIdAsync(int id)
        {
            return await _dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant?> GetTenantByEmailAsync(string email)
        {
            return await _dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Email.Value == email);
        }
    }
}
