using FluentValidation;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Leases.Commands.Create
{
    public class CreateLeaseCommandValidator : AbstractValidator<CreateLeaseCommand>
    {
        public CreateLeaseCommandValidator()
        {
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            RuleFor(x => x.ChargesAmount).GreaterThan(0m);
            RuleFor(x => x.RentAmount).GreaterThan(0m);
            RuleFor(x => x.Deposit).GreaterThan(0m);
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.ContentType).NotEmpty();
            RuleFor(x => x.FileSizeInBytes).GreaterThan(0);
            RuleFor(x => x.PropertyId).GreaterThan(0);
            RuleFor(x => x.Tenants).NotEmpty();

            RuleForEach(x => x.Tenants).SetValidator(new TenantCreateDtoValidator());
        }
    }
}
