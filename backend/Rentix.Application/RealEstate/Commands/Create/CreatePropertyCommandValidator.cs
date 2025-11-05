using FluentValidation;
using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.MaxRent).GreaterThan(0);
            RuleFor(x => x.RentNoCharges).GreaterThan(0);
            RuleFor(x => x.RentCharges).GreaterThan(0);
            RuleFor(x => x.Deposit).GreaterThan(0);
            RuleFor(x => x.Surface).GreaterThan(0);
            RuleFor(x => x.NumberRooms).InclusiveBetween(1, 150);
            RuleFor(x => x.LandLordId).NotEmpty();

            RuleFor(x => x)
                .Must(cmd => cmd.AddressId.HasValue || cmd.AddressDto != null)
                .WithMessage("Either AddressId or AddressDto must be provided.");

            When(x => x.AddressId.HasValue, () =>
            {
                RuleFor(x => x.AddressId!.Value)
                    .GreaterThan(0)
                    .WithMessage("AddressId must be greather than 0.");
            });

            When(x => x.AddressDto != null, () =>
            {
                RuleFor(x => x.AddressDto!).ChildRules(address =>
                {
                    address.RuleFor(a => a.Street).NotEmpty().MaximumLength(255);
                    address.RuleFor(a => a.City).NotEmpty().MaximumLength(100);
                    address.RuleFor(a => a.PostalCode).NotEmpty().MaximumLength(20);
                    address.RuleFor(a => a.Country).NotEmpty().MaximumLength(100);
                    address.RuleFor(a => a.Complement).MaximumLength(255);
                });
            });
        }
    }
}
