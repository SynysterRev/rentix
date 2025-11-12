using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Rentix.Infrastructure.Persistence;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    public class LeaseRepository : ILeaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LeaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Lease?> GetByIdAsync(int id)
        {
            return await _dbContext.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenants)
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Lease>> GetAllAsync()
        {
            return await _dbContext.Leases
            .Include(l => l.Property)
            .Include(l => l.Tenants)
            .Include(l => l.Payments)
            .ToListAsync();
        }

        public async Task<Lease> AddAsync(Lease lease)
        {
            await _dbContext.Leases.AddAsync(lease);
            return lease;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lease = await _dbContext.Leases.FindAsync(id);
            if (lease == null)
                return false;
            _dbContext.Leases.Remove(lease);
            return true;
        }
    }
}
