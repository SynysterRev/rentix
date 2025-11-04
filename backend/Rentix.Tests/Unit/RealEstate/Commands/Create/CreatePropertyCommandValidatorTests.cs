using FluentAssertions;
using Rentix.Application.RealEstate.Commands.Create;
using Xunit;
using System;
using Rentix.Domain.Entities;

namespace Rentix.Tests.Unit.RealEstate.Commands.Create
{
    public class CreatePropertyCommandValidatorTests
    {
        [Fact]
        public void Should_HaveValidationError_When_RequiredFieldsAreMissingOrInvalid()
        {
            // Arrange
            var validator = new CreatePropertyCommandValidator();
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
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        //[Fact]
        //public void Should_PassValidation_When_AllFieldsAreValid()
        //{
        //    // Arrange
        //    var validator = new CreatePropertyCommandValidator();
        //    var command = new CreatePropertyCommand
        //    {
        //        Name = "Valid Property",
        //        MaxRent = 1000m,
        //        RentCharges = 200m,
        //        RentNoCharges = 800m, // Not validated, but included for completeness
        //        Deposit = 500m,
        //        PropertyStatus = PropertyStatus.Available,
        //        Surface = 50m,
        //        NumberRooms = 2,
        //        AddressId = 1,
        //        LandLordId = Guid.NewGuid()
        //    };

        //    // Act
        //    var result = validator.Validate(command);

        //    // Assert
        //    result.IsValid.Should().BeTrue();
        //    result.Errors.Should().BeEmpty();
        //}

        // Boundary value tests for numeric fields
        [Theory]
        [InlineData(0)] // Min boundary
        [InlineData(-1)] // Below min
        public void Should_HaveValidationError_When_MaxRent_IsInvalid(int maxRent)
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = maxRent,
                Surface = 50,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.NewGuid(),
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_HaveValidationError_When_Surface_IsInvalid(int surface)
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000,
                Surface = surface,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.NewGuid(),
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_HaveValidationError_When_NumberRooms_IsInvalid(int numberRooms)
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000,
                Surface = 50,
                NumberRooms = numberRooms,
                AddressId = 1,
                LandLordId = Guid.NewGuid(),
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Should_HaveValidationError_When_AddressId_IsInvalid(int addressId)
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000,
                Surface = 50,
                NumberRooms = 2,
                AddressId = addressId,
                LandLordId = Guid.NewGuid(),
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        // Individual field validation tests
        [Fact]
        public void Should_HaveValidationError_When_Name_IsEmpty()
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "",
                MaxRent = 1000,
                Surface = 50,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.NewGuid(),
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_HaveValidationError_When_LandLordId_IsEmpty()
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000,
                Surface = 50,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.Empty,
                PropertyStatus = PropertyStatus.Available
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_HaveValidationError_When_PropertyStatus_IsMissing()
        {
            var validator = new CreatePropertyCommandValidator();
            var command = new CreatePropertyCommand
            {
                Name = "Valid Property",
                MaxRent = 1000,
                Surface = 50,
                NumberRooms = 2,
                AddressId = 1,
                LandLordId = Guid.NewGuid(),
                // PropertyStatus not set
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }
    }
}
