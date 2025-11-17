using Rentix.Application.Common.Interfaces;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.Mappers
{
    public class PropertyMapper : IPropertyMapper
    {
        private readonly IFileStorageService _fileStorageService;

        public PropertyMapper(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public PropertyDetailDto Map(Property property)
        {
            var activeLease = property.Leases?.FirstOrDefault(l => l.IsActive);

            return new PropertyDetailDto
            {
                Id = property.Id,
                Name = property.Name,
                MaxRent = property.MaxRent,
                RentWithoutCharges = property.RentNoCharges,
                RentCharges = property.RentCharges,
                Deposit = activeLease?.Deposit ?? property.Deposit,
                LeaseStartDate = activeLease?.StartDate ?? default,
                LeaseEndDate = activeLease?.EndDate ?? default,
                PropertyStatus = property.Status,
                Surface = property.Surface,
                NumberRooms = property.NumberRooms,
                Tenants = activeLease?.Tenants != null
                    ? activeLease.Tenants.Select(t => new TenantDto(
                        t.Id,
                        t.FirstName,
                        t.LastName,
                        t.Email.Value,
                        t.Phone.Value
                    )).ToList()
                    : new List<TenantDto>(),
                Address = property.Address != null
                    ? AddressDto.FromEntity(property.Address)
                    : null!,
                Documents = property.Documents != null
                    ? property.Documents.Select(d =>
                    {
                        string downloadUrl = _fileStorageService.GetPublicUrl(d.Id);
                        return new DocumentDto(
                            d.Id,
                            d.FileName,
                            d.DocumentType,
                            d.Description,
                            d.UploadAt,
                            downloadUrl
                        );
                    }).ToList()
                    : new List<DocumentDto>()
            };
        }
    }
}
