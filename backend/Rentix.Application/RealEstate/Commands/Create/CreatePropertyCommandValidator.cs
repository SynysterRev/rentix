using FluentValidation;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.MaxRent).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PropertyStatus).NotEmpty();
            RuleFor(x => x.Surface).NotEmpty().GreaterThan(0); ;
            RuleFor(x => x.NumberRooms).NotEmpty().InclusiveBetween(1, 150);
            RuleFor(x => x.AddressId).NotEmpty();
            RuleFor(x => x.LandLordId).NotEmpty();
        }
    }
}
