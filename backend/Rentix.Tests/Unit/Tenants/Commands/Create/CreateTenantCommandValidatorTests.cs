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
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
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
                TenantData = new TenantCreateDto { FirstName = value, LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.FirstName");
        }

        [Fact]
        public void Should_HaveValidationError_When_FirstNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = new string('A', 101), LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
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
                TenantData = new TenantCreateDto { FirstName = "John", LastName = value, Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_LastNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = new string('A', 101), Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_EmailIsNull()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = null!, PhoneNumber = "0601020304" }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.Email");
        }

        [Fact]
        public void Should_HaveValidationError_When_PhoneIsNull()
        {
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = null! }
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("TenantData.PhoneNumber");
        }
    }
}
