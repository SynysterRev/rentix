using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    public interface IPropertyRepository
    {
        public Task<IEnumerable<Property>> GetAllAsync();
        public Task<Property?> GetByIdAsync(int id);
        public Task<Property?> GetByIdWithDetailsAsync(int id);
        public Task<IEnumerable<Property>> GetByLandlordIdAsync(Guid landlordId);
        public Task<IEnumerable<Property>> GetAvailablePropertiesAsync();
        public Task<bool> ExistsAsync(int id);
        public Task<int> CountAsync();

        public Task<Property> AddAsync(Property property);
        public Task UpdateAsync(Property property);
        public Task<bool> DeleteAsync(int id);

        public Task<IEnumerable<Property>> SearchAsync(string searchTerm);
    }
}
