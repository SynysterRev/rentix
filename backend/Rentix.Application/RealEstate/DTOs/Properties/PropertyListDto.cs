using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Properties
{
    public record PropertyListDto(
        int Id,
        string Name,
        decimal TotalRent,
        List<string> TenantsNames,
        PropertyStatus PropertyStatus,
        AddressDto Address
        )
    {
        public bool IsAvailable => PropertyStatus == PropertyStatus.Available;
    }
}
