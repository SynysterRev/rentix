using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.Commands.Update;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Xunit;

namespace Rentix.Tests.Unit.RealEstate.Commands.Update
{
    public class UpdatePropertyCommandHandlerTests
    {
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly UpdatePropertyCommandHandler _handler;

        private static Property CreateTestProperty(int id = 1, string name = "Old Name")
        {
            return Property.Create(
                name: name,
                maxRent: 1000,
                deposit: 200,
                rentNoCharges: 800,
                rentCharges: 200,
                status: PropertyStatus.Available,
                surface: 60,
                numberRooms: 3,
                addressId: 1,
                landlordId: Guid.NewGuid()
            );
        }

        private static Address CreateTestAddress(int id = 1, string street = "Old Street")
        {
            var address = Address.Create(
                street: street,
                postalCode: "00000",
                city: "Old City",
                country: "Old Country",
                complement: "Old Complement"
            );
            address.Id = id;
            return address;
        }

        public UpdatePropertyCommandHandlerTests()
        {
            _handler = new UpdatePropertyCommandHandler(_propertyRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_When_PropertyDoesNotExist()
        {
            // Arrange
            var command = new UpdatePropertyCommand(99) { Name = "Name" };
            _propertyRepositoryMock.Setup(r => r.GetPropertyByIdAsync(99)).ReturnsAsync((Property)null!);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Property with ID 99 not found");
        }

        [Fact]
        public async Task Should_UpdateProperty_When_FieldsAreProvided()
        {
            // Arrange
            var property = CreateTestProperty();
            property.Id = 1;
            var command = new UpdatePropertyCommand(1)
            {
                Name = "New Name",
                RentNoCharges = 200,
                RentCharges = 100,
                Deposit = 300,
                Surface = 80,
                NumberRooms = 4
            };
            _propertyRepositoryMock.Setup(r => r.GetPropertyByIdAsync(1)).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Name.Should().Be("New Name");
            result.RentWithoutCharges.Should().Be(200);
            result.RentCharges.Should().Be(100);
            result.Deposit.Should().Be(300);
            result.Surface.Should().Be(80);
            result.NumberRooms.Should().Be(4);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Should_UpdateAddress_When_AddressIsProvided()
        {
            // Arrange
            var address = CreateTestAddress();
            var property = CreateTestProperty();
            property.Id = 1;
            property.Address = address;
            var command = new UpdatePropertyCommand(1)
            {
                Address = new AddressUpdateDto(1, "New Street", "New City", "12345", "New Country", "New Complement")
            };
            _propertyRepositoryMock.Setup(r => r.GetPropertyByIdAsync(1)).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Address.Street.Should().Be("New Street");
            result.Address.City.Should().Be("New City");
            result.Address.PostalCode.Should().Be("12345");
            result.Address.Country.Should().Be("New Country");
            result.Address.Complement.Should().Be("New Complement");
        }

        [Fact]
        public async Task Should_NotUpdateFields_When_FieldsAreNull()
        {
            // Arrange
            var property = CreateTestProperty();
            property.Id = 1;
            var command = new UpdatePropertyCommand(1);
            _propertyRepositoryMock.Setup(r => r.GetPropertyByIdAsync(1)).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Name.Should().Be("Old Name");
            result.RentWithoutCharges.Should().Be(800);
            result.RentCharges.Should().Be(200);
            result.Deposit.Should().Be(200);
            result.Surface.Should().Be(60);
            result.NumberRooms.Should().Be(3);
        }

        [Fact]
        public async Task Should_ThrowException_When_SaveChangesFails()
        {
            // Arrange
            var property = CreateTestProperty();
            property.Id = 1;
            var command = new UpdatePropertyCommand(1) { Name = "Name" };
            _propertyRepositoryMock.Setup(r => r.GetPropertyByIdAsync(1)).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
