using FluentValidation.TestHelper;
using Rentix.Application.RealEstate.Commands.Update;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Xunit;

namespace Rentix.Tests.Unit.RealEstate.Commands.Update
{
    public class UpdatePropertyCommandValidatorTests
    {
        private readonly UpdatePropertyCommandValidator _validator = new();

        [Fact]
        public void Should_PassValidation_When_AllFieldsAreNull()
        {
            var command = new UpdatePropertyCommand(1);
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_HaveValidationError_When_NameIsEmpty()
        {
            var command = new UpdatePropertyCommand(1) { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public void Should_HaveValidationError_When_NameIsTooLong()
        {
            var command = new UpdatePropertyCommand(1) { Name = new string('A', 256) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_RentNoChargesIsInvalid(decimal value)
        {
            var command = new UpdatePropertyCommand(1) { RentNoCharges = value };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.RentNoCharges);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_RentChargesIsInvalid(decimal value)
        {
            var command = new UpdatePropertyCommand(1) { RentCharges = value };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.RentCharges);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_DepositIsInvalid(decimal value)
        {
            var command = new UpdatePropertyCommand(1) { Deposit = value };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Deposit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_SurfaceIsInvalid(decimal value)
        {
            var command = new UpdatePropertyCommand(1) { Surface = value };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Surface);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(151)]
        public void Should_HaveValidationError_When_NumberRoomsIsInvalid(int value)
        {
            var command = new UpdatePropertyCommand(1) { NumberRooms = value };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.NumberRooms);
        }

        [Fact]
        public void Should_PassValidation_When_AddressIsNull()
        {
            var command = new UpdatePropertyCommand(1) { Address = null };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public void Should_HaveValidationError_When_AddressStreetIsEmptyOrTooLong()
        {
            var address = new AddressUpdateDto(1, "", "City", "12345", "Country", null);
            var command = new UpdatePropertyCommand(1) { Address = address };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.Street");

            address = new AddressUpdateDto(1, new string('A', 256), "City", "12345", "Country", null);
            command = new UpdatePropertyCommand(1) { Address = address };
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.Street");
        }

        [Fact]
        public void Should_HaveValidationError_When_AddressCityIsEmptyOrTooLong()
        {
            var address = new AddressUpdateDto(1, "Street", "", "12345", "Country", null);
            var command = new UpdatePropertyCommand(1) { Address = address };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.City");

            address = new AddressUpdateDto(1, "Street", new string('A', 101), "12345", "Country", null);
            command = new UpdatePropertyCommand(1) { Address = address };
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.City");
        }

        [Fact]
        public void Should_HaveValidationError_When_AddressPostalCodeIsEmptyOrTooLong()
        {
            var address = new AddressUpdateDto(1, "Street", "City", "", "Country", null);
            var command = new UpdatePropertyCommand(1) { Address = address };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.PostalCode");

            address = new AddressUpdateDto(1, "Street", "City", new string('A', 21), "Country", null);
            command = new UpdatePropertyCommand(1) { Address = address };
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.PostalCode");
        }

        [Fact]
        public void Should_HaveValidationError_When_AddressCountryIsEmptyOrTooLong()
        {
            var address = new AddressUpdateDto(1, "Street", "City", "12345", "", null);
            var command = new UpdatePropertyCommand(1) { Address = address };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.Country");

            address = new AddressUpdateDto(1, "Street", "City", "12345", new string('A', 101), null);
            command = new UpdatePropertyCommand(1) { Address = address };
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.Country");
        }

        [Fact]
        public void Should_HaveValidationError_When_AddressComplementIsTooLong()
        {
            var address = new AddressUpdateDto(1, "Street", "City", "12345", "Country", new string('A', 256));
            var command = new UpdatePropertyCommand(1) { Address = address };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Address.Complement");
        }

        [Fact]
        public void Should_PassValidation_When_AllFieldsAreValid()
        {
            var address = new AddressUpdateDto(1, "Street", "City", "12345", "Country", "Complement");
            var command = new UpdatePropertyCommand(1)
            {
                Name = "Valid Name",
                RentNoCharges = 100m,
                RentCharges = 50m,
                Deposit = 200m,
                Surface = 60m,
                NumberRooms = 3,
                Address = address
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
