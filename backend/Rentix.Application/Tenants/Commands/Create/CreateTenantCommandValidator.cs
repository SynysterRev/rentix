using FluentValidation;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Tenants.Commands.Create
{
    public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            RuleFor(x => x.TenantData)
            .NotNull()
            .SetValidator(new TenantCreateDtoValidator());
        }
    }
}
