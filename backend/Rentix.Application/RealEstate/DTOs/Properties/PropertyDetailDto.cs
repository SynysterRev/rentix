using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;
using System.Linq;

namespace Rentix.Application.RealEstate.DTOs.Properties
{
    public record PropertyDetailDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal MaxRent { get; init; }
        public decimal RentWithoutCharges { get; init; }
        public decimal RentCharges { get; init; }
        public decimal Deposit { get; init; }
        public DateTime LeaseStartDate { get; init; }
        public DateTime LeaseEndDate { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public decimal Surface { get; init; }
        public int NumberRooms { get; init; }
        public List<TenantDto> Tenants { get; init; } = null!;
        public AddressDto Address { get; init; } = null!;
        public List<DocumentDto> Documents { get; init; } = new List<DocumentDto>();

        public static PropertyDetailDto FromEntity(Property property)
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
                    ? property.Documents.Select(d => new DocumentDto(
                        d.Id,
                        d.FileName,
                        d.DocumentType,
                        d.FilePath,
                        d.Description,
                        d.UploadAt
                    )).ToList()
                    : new List<DocumentDto>()
            };
        }
    }
}
