using FluentAssertions;
using Xunit;
using Rentix.Domain.Entities;
using Rentix.Application.RealEstate.Commands.Create.Property;

namespace Rentix.Tests.Unit.RealEstate.Commands.Create
{
    public class CreatePropertyCommandValidatorTests
    {
        private readonly CreatePropertyCommandValidator _validator;

        public CreatePropertyCommandValidatorTests()
        {
            _validator = new CreatePropertyCommandValidator();
        }

        private CreatePropertyCommand CreateValidCommand()
        {
            return new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000m,
                RentCharges = 200m,
                RentNoCharges = 800m,
                Deposit = 500m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50m,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.NewGuid()
            };
        }

        [Fact]
        public void Should_HaveValidationError_When_RequiredFieldsAreMissingOrInvalid()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                Name = "",
                MaxRent = 0,
                Surface = 0,
                NumberRooms = 0,
                AddressId = 0,
                LandLordId = Guid.Empty
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_PassValidation_When_AllFieldsAreValid()
        {
            // Arrange
            var command = CreateValidCommand();

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        // Boundary value tests for numeric fields
        [Theory]
        [InlineData(0)] // Min boundary
        [InlineData(-1)] // Below min
        public void Should_HaveValidationError_When_MaxRent_IsInvalid(int maxRent)
        {
            var command = CreateValidCommand() with { MaxRent = maxRent };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_HaveValidationError_When_Surface_IsInvalid(int surface)
        {
            var command = CreateValidCommand() with { Surface = surface };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_NumberRooms_IsInvalid(int numberRooms)
        {
            var command = CreateValidCommand() with { NumberRooms = numberRooms };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Should_HaveValidationError_When_AddressId_IsInvalid(int addressId)
        {
            var command = CreateValidCommand() with { AddressId = addressId };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        // Individual field validation tests
        [Fact]
        public void Should_HaveValidationError_When_Name_IsEmpty()
        {
            var command = CreateValidCommand() with { Name = string.Empty };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_HaveValidationError_When_LandLordId_IsEmpty()
        {
            var command = CreateValidCommand() with { LandLordId = Guid.Empty };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_HaveValidationError_When_Name_IsTooLong()
        {
            var command = CreateValidCommand() with { Name = new string('A', 256) };
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }
    }
}
