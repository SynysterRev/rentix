using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.Tenants.Commands.Create
{
    public record CreateTenantCommand : IRequest<TenantDto>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public Email Email { get; init; } = null!;
        public Phone Phone { get; init; } = null!;
    }
}
