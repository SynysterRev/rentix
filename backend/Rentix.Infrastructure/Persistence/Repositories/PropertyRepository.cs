using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PropertyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Property> AddAsync(Property property)
        {
            _dbContext.Properties.Add(property);
            await _dbContext.SaveChangesAsync();
            return property;
        }

        //public async Task<int> CountAsync()
        //{
        //    return await _dbContext.Properties.CountAsync();
        //}

        public async Task<bool> DeleteAsync(int id)
        {
            var property = await _dbContext.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }

            _dbContext.Properties.Remove(property);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> ExistsAsync(int id)
        //{
        //    return await _dbContext.Properties.AnyAsync(p => p.Id == id);
        //}

        //public async Task<IEnumerable<Property>> GetAllAsync()
        //{
        //    return await _dbContext.Properties.ToListAsync();
        //}

        //public async Task<IEnumerable<Property>> GetAvailablePropertiesAsync()
        //{
        //    return await _dbContext.Properties.Where(p => p.Status == PropertyStatus.Available).ToListAsync();
        //}

        //public async Task<Property?> GetByIdAsync(int id)
        //{
        //    return await _dbContext.Properties
        //        .Include(p => p.Address)
        //        .FirstOrDefaultAsync(p => p.Id == id);
        //}

        //public async Task<Property?> GetByIdWithDetailsAsync(int id)
        //{
        //    return await _dbContext.Properties
        //        .Include(p => p.Leases)
        //            .ThenInclude(l => l.Tenants)
        //        .Include(p => p.Documents)
        //        .Include(p => p.Landlord)
        //        .Include(p => p.Address)
        //        .FirstOrDefaultAsync(p => p.Id == id);
        //}

        //public async Task<IEnumerable<Property>> GetByLandlordIdAsync(Guid landlordId)
        //{
        //    return await _dbContext.Properties.Where(p => p.LandlordId == landlordId).ToListAsync();
        //}

        //public Task<IEnumerable<Property>> SearchAsync(string searchTerm)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task UpdateAsync(Property property)
        {
            _dbContext.Properties.Update(property);
            await _dbContext.SaveChangesAsync();
        }
    }
}
