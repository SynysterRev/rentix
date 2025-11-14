using FluentAssertions;
using FluentValidation.TestHelper;
using Rentix.Application.Leases.Commands.Create;
using Rentix.Application.Tenants.DTOs.Tenants;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rentix.Tests.Unit.Leases.Commands.Create
{
    public class CreateLeaseCommandValidatorTests
    {
        private readonly CreateLeaseCommandValidator _validator = new();

        private CreateLeaseCommand CreateValidCommand()
        {
            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);

            return new CreateLeaseCommand
            {
                PropertyId = 1,
                StartDate = start,
                EndDate = end,
                RentAmount = 1000m,
                ChargesAmount = 100m,
                Deposit = 1000m,
                IsActive = true,
                Notes = "Test",
                FileStream = new MemoryStream(Encoding.UTF8.GetBytes("x")),
                FileName = "lease.pdf",
                ContentType = "application/pdf",
                FileSizeInBytes = 1,
                Tenants = new List<TenantCreateDto>
                {
                    new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
                }
            };
        }

        [Fact]
        public void Should_PassValidation_When_AllFieldsAreValid()
        {
            var command = CreateValidCommand();

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_HaveError_When_EndDateIsNotGreaterThanStartDate()
        {
            var command = CreateValidCommand() with { EndDate = CreateValidCommand().StartDate };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void Should_HaveError_When_RentAmountIsNotPositive()
        {
            var command = CreateValidCommand() with { RentAmount = 0m };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.RentAmount);
        }

        [Fact]
        public void Should_HaveError_When_ChargesAmountIsNotPositive()
        {
            var command = CreateValidCommand() with { ChargesAmount = 0m };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ChargesAmount);
        }

        [Fact]
        public void Should_HaveError_When_DepositIsNotPositive()
        {
            var command = CreateValidCommand() with { Deposit = 0m };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Deposit);
        }

        [Fact]
        public void Should_HaveError_When_FileNameIsEmpty()
        {
            var command = CreateValidCommand() with { FileName = string.Empty };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FileName);
        }

        [Fact]
        public void Should_HaveError_When_ContentTypeIsEmpty()
        {
            var command = CreateValidCommand() with { ContentType = string.Empty };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ContentType);
        }

        [Fact]
        public void Should_HaveError_When_FileSizeIsNotPositive()
        {
            var command = CreateValidCommand() with { FileSizeInBytes = 0L };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FileSizeInBytes);
        }

        [Fact]
        public void Should_HaveError_When_PropertyIdIsInvalid()
        {
            var command = CreateValidCommand() with { PropertyId = 0 };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.PropertyId);
        }

        [Fact]
        public void Should_HaveError_When_TenantsCollectionIsEmpty()
        {
            var command = CreateValidCommand() with { Tenants = new List<TenantCreateDto>() };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Tenants);
        }

        [Fact]
        public void Should_HaveError_When_TenantIsInvalid()
        {
            var command = CreateValidCommand();
            // make a tenant with invalid email and empty firstname
            command.Tenants.Clear();
            command.Tenants.Add(new TenantCreateDto { FirstName = "", LastName = "Doe", Email = "notanemail", PhoneNumber = "" });

            var result = _validator.TestValidate(command);

            // Expect at least one validation error related to Tenants collection elements
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Tenants"));
        }
    }
}