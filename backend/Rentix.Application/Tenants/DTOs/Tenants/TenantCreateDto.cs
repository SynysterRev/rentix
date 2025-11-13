using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.Tenants.DTOs.Tenants
{
    public record TenantCreateDto(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber);
}
