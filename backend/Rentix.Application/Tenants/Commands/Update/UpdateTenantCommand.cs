using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.Tenants.Commands.Update
{
    public record UpdateTenantCommand(int TenantId) : IRequest<TenantDto>
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
    }
}
