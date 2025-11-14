using FluentValidation.TestHelper;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.DTOs.Tenants;
using Xunit;

namespace Rentix.Tests.Unit.Tenants.Commands.Create
{
    public class CreateTenantCommandValidatorTests
    {
        private readonly CreateTenantCommandValidator _validator = new();

        [Fact]
        public void Should_PassValidation_When_AllFieldsAreValid()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                "John",
                "Doe",
                "john@doe.com",
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        public void Should_HaveValidationError_When_FirstNameIsEmptyOrNull(string value)
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                value,
                "Doe",
                "john@doe.com",
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.FirstName");
        }

        [Fact]
        public void Should_HaveValidationError_When_FirstNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                new string('A', 101),
                "Doe",
                "john@doe.com",
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.FirstName");
        }

        [Theory]
        [InlineData("")]
        public void Should_HaveValidationError_When_LastNameIsEmptyOrNull(string value)
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                "John",
                value,
                "john@doe.com",
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_LastNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                "John",
                new string('A', 101),
                "john@doe.com",
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_EmailIsNull()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                "John",
                "Doe",
                null!,
                "0601020304"
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.Email");
        }

        [Fact]
        public void Should_HaveValidationError_When_PhoneIsNull()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto(
                "John",
                "Doe",
                "john@doe.com",
                null!
                )
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.PhoneNumber");
        }
    }
}
