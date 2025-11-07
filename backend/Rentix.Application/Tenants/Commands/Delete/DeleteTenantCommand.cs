using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.Tenants.Commands.Delete
{
    public record DeleteTenantCommand(int TenantId) : IRequest
    {
    }
}
