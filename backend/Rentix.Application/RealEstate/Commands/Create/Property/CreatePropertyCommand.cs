using MediatR;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.Commands.Create.Property
{
    public record CreatePropertyCommand() : IRequest<PropertyDetailDto>
    {
        public string Name { get; init; } = string.Empty;
        public decimal MaxRent { get; init; }
        public decimal RentNoCharges { get; init; }
        public decimal RentCharges { get; init; }
        public decimal Deposit { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public decimal Surface { get; init; }
        public int NumberRooms { get; init; }
        public int? AddressId { get; init; }
        public AddressCreateDto? AddressDto { get; init; }
        public Guid LandLordId { get; init; }
    }
}
