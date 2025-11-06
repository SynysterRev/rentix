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

        public void Update(Tenant tenant)
        {
            _dbContext.Tenants.Update(tenant);
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
    }
}
