using Rentix.Domain.ValueObjects;

namespace Rentix.Application.RealEstate.DTOs.Tenants
{
    public record TenantDto(
        int Id,
        string FirstName,
        string LastName,
        Email Email,
        Phone PhoneNumber)
    {
        public string FullName => FirstName + " " + LastName;
    }
}
