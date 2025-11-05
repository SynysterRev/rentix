using FluentValidation;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.MaxRent).NotEmpty().GreaterThan(0);
            RuleFor(x => x.RentNoCharges).NotEmpty().GreaterThan(0);
            RuleFor(x => x.RentCharges).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Deposit).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PropertyStatus).NotEmpty();
            RuleFor(x => x.Surface).NotEmpty().GreaterThan(0);
            RuleFor(x => x.NumberRooms).NotEmpty().InclusiveBetween(1, 150);
            RuleFor(x => x.LandLordId).NotEmpty();

            RuleFor(x => x)
                .Must(cmd => cmd.AddressId.HasValue || cmd.AddressDto != null)
                .WithMessage("Either AddressId or AddressDto must be provided.");

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
