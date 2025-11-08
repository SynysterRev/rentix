using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Domain.Entities;
using Rentix.Domain.ValueObjects;

namespace Rentix.Application.Tenants.DTOs.Tenants
{
    public record TenantDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber)
    {
        public string FullName => FirstName + " " + LastName;

        public static TenantDto FromEntity(Tenant tenant)
        {
            ArgumentNullException.ThrowIfNull(tenant);

            return new TenantDto
            (
                tenant.Id,
                tenant.FirstName,
                tenant.LastName,
                tenant.Email.Value,
                tenant.Phone.Value
            );
        }
    }
}
