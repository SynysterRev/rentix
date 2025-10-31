using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs
{
    public record AddressDto(
        int Id,
        string Street,
        string City,
        string PostalCode,
        string Country,
        string? Complement)
    {
        public string FullAddress => $"{Street}, {PostalCode} {City}, {Country}";

        public static AddressDto FromEntity(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);

            return new AddressDto
            (
                address.Id,
                address.Street,
                address.City,
                address.PostalCode,
                address.Country,
                address.Complement
            );
        }
    }
}
