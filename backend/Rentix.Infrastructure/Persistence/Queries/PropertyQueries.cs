using Microsoft.EntityFrameworkCore;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Infrastructure.Persistence.Queries
{
    public class PropertyQueries : IPropertyQueries
    {
        private readonly ApplicationDbContext _dbContext;

        public PropertyQueries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PropertyListDto>> GetPropertyListAsync()
        {
            return await _dbContext.Properties
                .AsNoTracking()
                .Select(p => new PropertyListDto(
                    p.Id,
                    p.Name,
                    p.RentCharges + p.RentNoCharges,
                    p.Leases
                    .Where(l => l.IsActive)
                    .SelectMany(l => l.Tenants)
                    .Select(t => t.FirstName + " " + t.LastName)
                    .ToList(),
                    p.Status,
                    AddressDto.FromEntity(p.Address)
                ))
                .ToListAsync();
        }
    }
}
