using FluentValidation.TestHelper;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Domain.ValueObjects;
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
                FirstName = "John",
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
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
                FirstName = value,
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("FirstName");
        }

        [Fact]
        public void Should_HaveValidationError_When_FirstNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                FirstName = new string('A', 101),
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("FirstName");
        }

        [Theory]
        [InlineData("")]
        public void Should_HaveValidationError_When_LastNameIsEmptyOrNull(string value)
        {
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = value,
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_LastNameIsTooLong()
        {
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = new string('A', 101),
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("LastName");
        }

        [Fact]
        public void Should_HaveValidationError_When_EmailIsNull()
        {
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = null!,
                Phone = Phone.Create("0601020304")
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Email");
        }

        [Fact]
        public void Should_HaveValidationError_When_PhoneIsNull()
        {
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = null!
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Phone");
        }
    }
}
