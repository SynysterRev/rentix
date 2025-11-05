using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Addresses
{
    public record AddressCreateDto(
        string Street,
        string City,
        string PostalCode,
        string Country,
        string? Complement)
    {
        /// <summary>
        /// Converts this AddressCreateDto to an Address entity using the Address factory.
        /// </summary>
        /// <returns>An Address entity created from the DTO values.</returns>
        public Address ToEntity()
        {
            return Address.Create(Street, PostalCode, City, Country, Complement);
        }
    }
}