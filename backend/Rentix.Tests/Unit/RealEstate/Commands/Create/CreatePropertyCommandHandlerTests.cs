using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Rentix.API.Controllers.v1;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.Commands.Create.Property;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Mappers;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Rentix.Tests.Unit.RealEstate.Commands.Create
{
    public class CreatePropertyCommandHandlerTests
    {
        private readonly Mock<IAddressRepository> _addressRepoMock;
        private readonly Mock<IPropertyRepository> _propertyRepoMock;
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly Mock<ILogger<CreatePropertyCommandHandler>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CreatePropertyCommandHandlerTests()
        {
            _addressRepoMock = new Mock<IAddressRepository>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fileStorageMock = new Mock<IFileStorageService>();
            _loggerMock = new Mock<ILogger<CreatePropertyCommandHandler>>();
        }

        private CreatePropertyCommandHandler CreateHandler()
        {
            var mapper = new PropertyMapper(_fileStorageMock.Object);
            return new CreatePropertyCommandHandler(_propertyRepoMock.Object, _addressRepoMock.Object, _unitOfWorkMock.Object, mapper, _loggerMock.Object);
        }

        private CreatePropertyCommand CreateValidCommand(int addressId, Guid landlordId)
        {
            return new CreatePropertyCommand
            {
                Name = "Test Property",
                MaxRent = 1000,
                Deposit = 500,
                RentNoCharges = 800,
                RentCharges = 200,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                AddressId = addressId,
                LandLordId = landlordId
            };
        }

        [Fact]
        public async Task Handle_WithValidAddressId_ShouldCreatePropertyAndReturnPropertyDto()
        {
            // Arrange
            var address = Address.Create("Street", "Postal", "City", "Country", null);
            address.Id = 1;
            var property = Property.Create(
                "Test Property",
                1000,
                500,
                800,
                200,
                PropertyStatus.Available,
                50,
                2,
                address.Id,
                Guid.NewGuid()
            );
            property.Id = 42;

            _addressRepoMock.Setup(r => r.GetByIdAsync(address.Id)).ReturnsAsync(address);
            _propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>())).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            var handler = CreateHandler();
            var command = CreateValidCommand(address.Id, property.LandlordId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(property.Id);
            result.Name.Should().Be(property.Name);
            result.Address.Should().NotBeNull();
            result.Address.Street.Should().Be(address.Street);
            result.Address.City.Should().Be(address.City);
            result.Address.PostalCode.Should().Be(address.PostalCode);
            result.Address.Country.Should().Be(address.Country);

            _addressRepoMock.Verify(r => r.GetByIdAsync(address.Id), Times.Once);
            _propertyRepoMock.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentAddressId_ShouldThrowNotFoundException()
        {
            // Arrange
            _addressRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Address?)null);
            var handler = CreateHandler();
            var command = CreateValidCommand(999, Guid.NewGuid());

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Address with ID 999 not found");
        }

        [Fact]
        public async Task Handle_WithAddressDto_ShouldCreateAddressAndPropertyAndReturnPropertyDto()
        {
            // Arrange
            var address = Address.Create("Street", "Postal", "City", "Country", null);
            address.Id = 2;
            var property = Property.Create(
                "Test Property",
                1000,
                500,
                800,
                200,
                PropertyStatus.Available,
                50,
                2,
                address.Id,
                Guid.NewGuid()
            );
            property.Id = 43;

            _addressRepoMock.Setup(r => r.AddAsync(It.IsAny<Address>())).ReturnsAsync(address);
            _propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>())).ReturnsAsync(property);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);
            _fileStorageMock.Setup(s => s.GetPublicUrl(It.IsAny<int>())).Returns("fake-url");

            var handler = CreateHandler();
            var command = new CreatePropertyCommand
            {
                Name = property.Name,
                MaxRent = property.MaxRent,
                Deposit = property.Deposit,
                RentNoCharges = property.RentNoCharges,
                RentCharges = property.RentCharges,
                PropertyStatus = property.Status,
                Surface = property.Surface,
                NumberRooms = property.NumberRooms,
                AddressDto = new AddressCreateDto(
                    address.Street, address.City, address.PostalCode, address.Country, address.Complement),
                LandLordId = property.LandlordId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(property.Id);
            result.Name.Should().Be(property.Name);
            result.Address.Should().NotBeNull();
            result.Address.Street.Should().Be(address.Street);
            result.Address.City.Should().Be(address.City);
            result.Address.PostalCode.Should().Be(address.PostalCode);
            result.Address.Country.Should().Be(address.Country);

            _addressRepoMock.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
            _propertyRepoMock.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_WithoutAddressIdOrDto_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var handler = CreateHandler();
            var command = new CreatePropertyCommand
            {
                Name = "Test",
                MaxRent = 1000,
                Deposit = 500,
                RentNoCharges = 800,
                RentCharges = 200,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                LandLordId = Guid.NewGuid()
                // No AddressId, no AddressDto
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Address must be resolved before creating a property.");
        }

        [Fact]
        public async Task Handle_WithAddressDtoMissingFields_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var handler = CreateHandler();
            var command = new CreatePropertyCommand
            {
                Name = "Test",
                MaxRent = 1000,
                Deposit = 500,
                RentNoCharges = 800,
                RentCharges = 200,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                AddressDto = new AddressCreateDto("", "", "", "", null),
                LandLordId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
