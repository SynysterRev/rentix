using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _dbContext.Addresses.FindAsync(id);
        }

        public async Task<Address> AddAsync(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            return address;
        }

        public void Update(Address address)
        {
            _dbContext.Addresses.Update(address);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await _dbContext.Addresses.FindAsync(id);
            if (address == null)
                return false;

            _dbContext.Addresses.Remove(address);
            return true;
        }

    }
}