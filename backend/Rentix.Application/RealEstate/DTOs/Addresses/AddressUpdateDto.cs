using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.RealEstate.DTOs.Addresses
{
    public record AddressUpdateDto(
        int Id,
        string? Street,
        string? City,
        string? PostalCode,
        string? Country,
        string? Complement);
}
