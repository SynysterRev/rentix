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
            await _dbContext.Properties.AddAsync(property);
            return property;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var property = await _dbContext.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }

            _dbContext.Properties.Remove(property);
            return true;
        }

        public void Update(Property property)
        {
            _dbContext.Properties.Update(property);
        }
    }
}
