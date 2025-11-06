using FluentValidation;

namespace Rentix.Application.Tenants.Commands.Create
{
    public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotNull();

            RuleFor(x => x.Phone)
                .NotNull();
        }
    }
}
