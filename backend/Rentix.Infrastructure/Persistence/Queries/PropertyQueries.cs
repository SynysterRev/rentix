using Microsoft.EntityFrameworkCore;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Common.Interfaces.Queries;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.Tenants.DTOs.Tenants;
using System.Linq;

namespace Rentix.Infrastructure.Persistence.Queries
{
    public class PropertyQueries : IPropertyQueries
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public PropertyQueries(ApplicationDbContext dbContext, IFileStorageService fileStorageService)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
        }

        public async Task<bool> ExistsAsync(int propertyId)
        {
            return await _dbContext.Properties.AnyAsync(p => p.Id == propertyId);
        }

        public async Task<PropertyDetailDto?> GetPropertyByIdAsync(int propertyId)
        {
            return await _dbContext.Properties
                .AsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => new PropertyDetailDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Surface = p.Surface,
                    MaxRent = p.MaxRent,
                    NumberRooms = p.NumberRooms,
                    RentCharges = p.RentCharges,
                    RentWithoutCharges = p.RentNoCharges,
                    Deposit = p.Deposit,
                    PropertyStatus = p.Status,
                    Address = AddressDto.FromEntity(p.Address),
                    Tenants = p.Leases
                        .Where(l => l.IsActive)
                        .SelectMany(l => l.Tenants)
                        .Select(t => new TenantDto(
                            t.Id,
                            t.FirstName,
                            t.LastName,
                            t.Email.Value,
                            t.Phone.Value))
                        .ToList(),
                    LeaseStartDate = p.Leases
                        .Where(l => l.IsActive)
                        .Select(l => l.StartDate)
                        .FirstOrDefault(),
                    LeaseEndDate = p.Leases
                        .Where(l => l.IsActive)
                        .Select(l => l.EndDate)
                        .FirstOrDefault(),
                    Documents = p.Documents
                        .Select(d => new DocumentDto(
                            d.Id, 
                            d.FileName, 
                            d.DocumentType,
                            d.Description, 
                            d.UploadAt,
                            _fileStorageService.GetPublicUrl(d.Id)))
                        .ToList(),
                })
                .FirstOrDefaultAsync();
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
