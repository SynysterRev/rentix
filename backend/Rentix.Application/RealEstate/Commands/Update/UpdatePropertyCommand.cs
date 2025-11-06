using MediatR;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.Commands.Update
{
    public record UpdatePropertyCommand(int propertyId) : IRequest<PropertyDetailDto>
    {
        public string? Name { get; set; }
        public decimal? RentNoCharges { get; set; }
        public decimal? RentCharges { get; set; }
        public decimal? Deposit { get; set; }
        public PropertyStatus? Status { get; set; }
        public decimal? Surface { get; set; }
        public int? NumberRooms { get; set; }

        public AddressUpdateDto? Address { get; set; }
    }
}
