using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Properties
{
    public record PropertyDetailDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal MaxRent { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public decimal Surface { get; init; }
        public int NumberRooms { get; init; }
        public AddressDto Address { get; init; } = null!;


    }
}
