using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs
{
    public record PropertyDto(
        int Id,
        string Name,
        decimal MaxRent,
        PropertyStatus PropertyStatus,
        decimal Surface,
        int NumberRooms,
        AddressDto address
        )
    {
        public bool IsAvailable => PropertyStatus == PropertyStatus.Available;

        public static PropertyDto FromEntity(Property property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return new PropertyDto
            (
                property.Id,
                property.Name,
                property.MaxRent,
                property.Status,
                property.Surface,
                property.NumberRooms,
                AddressDto.FromEntity(property.Address)
            );
        }
    }
}
