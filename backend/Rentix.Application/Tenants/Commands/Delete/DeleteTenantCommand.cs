using MediatR;

namespace Rentix.Application.Tenants.Commands.Delete
{
    public record DeleteTenantCommand(int TenantId) : IRequest
    {
    }
}
