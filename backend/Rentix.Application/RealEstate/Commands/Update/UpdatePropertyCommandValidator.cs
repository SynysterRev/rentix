using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Application.RealEstate.Commands.Update
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name!).NotEmpty().MaximumLength(255);
            });

            When(x => x.RentNoCharges.HasValue, () =>
            {
                RuleFor(x => x.RentNoCharges!.Value)
                .GreaterThan(0)
                .OverridePropertyName("RentNoCharges");
            });

            When(x => x.RentCharges.HasValue, () =>
            {
                RuleFor(x => x.RentCharges!.Value)
                .GreaterThan(0)
                .OverridePropertyName("RentCharges");
            });

            When(x => x.Deposit.HasValue, () =>
            {
                RuleFor(x => x.Deposit!.Value)
                .GreaterThan(0)
                .OverridePropertyName("Deposit");
            });

            When(x => x.Surface.HasValue, () =>
            {
                RuleFor(x => x.Surface!.Value)
                .GreaterThan(0)
                .OverridePropertyName("Surface");
            });

            When(x => x.NumberRooms.HasValue, () =>
            {
                RuleFor(x => x.NumberRooms!.Value)
                .InclusiveBetween(1, 150)
                .OverridePropertyName("NumberRooms");
            });

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address!).ChildRules(address =>
                {
                    address.When(a => a.Street is not null, () =>
                    {
                        address.RuleFor(a => a.Street).NotEmpty().MaximumLength(255);
                    });
                    address.When(a => a.City is not null, () =>
                    {
                        address.RuleFor(a => a.City).NotEmpty().MaximumLength(100);
                    });
                    address.When(a => a.PostalCode is not null, () =>
                    {
                        address.RuleFor(a => a.PostalCode).NotEmpty().MaximumLength(20);
                    });
                    address.When(a => a.Country is not null, () =>
                    {
                        address.RuleFor(a => a.Country).NotEmpty().MaximumLength(100);
                    });
                    address.When(a => a.Complement is not null, () =>
                    {
                        address.RuleFor(a => a.Complement).MaximumLength(255);
                    });
                });
            });
        }
    }
}
