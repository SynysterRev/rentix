using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.RealEstate.Queries.Detail;
using Rentix.Domain.Entities;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Tests.Unit.RealEstate.Queries.Detail
{
    public class DetailPropertyQueryHandlerTests
    {
        private readonly Mock<IPropertyQueries> _propertyQueriesMock;
        private readonly DetailPropertyQueryHandler _handler;

        public DetailPropertyQueryHandlerTests()
        {
            _propertyQueriesMock = new Mock<IPropertyQueries>();
            _handler = new DetailPropertyQueryHandler(_propertyQueriesMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPropertyDetail_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            var expectedProperty = new PropertyDetailDto
            {
                Id = propertyId,
                Name = "Test Property",
                MaxRent = 1000,
                RentWithoutCharges = 800,
                RentCharges = 200,
                Deposit = 500,
                LeaseStartDate = DateTime.Today,
                LeaseEndDate = DateTime.Today.AddYears(1),
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                Tenants = new List<TenantDto>(),
                Address = null!,
                Documents = new List<DocumentDto>()
            };
            _propertyQueriesMock.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ReturnsAsync(expectedProperty);
            var query = new DetailPropertyQuery(propertyId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedProperty);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 2;
            _propertyQueriesMock.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ReturnsAsync((PropertyDetailDto?)null);
            var query = new DetailPropertyQuery(propertyId);

            // Act & Assert
            await _handler.Invoking(h => h.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Property with ID {propertyId} does not exist");
        }

        [Fact]
        public async Task Handle_ReturnsPropertyDetail_WithExtremeValues()
        {
            // Arrange
            var propertyId = 3;
            var expectedProperty = new PropertyDetailDto
            {
                Id = propertyId,
                Name = string.Empty,
                MaxRent = decimal.MaxValue,
                RentWithoutCharges = decimal.MinValue,
                RentCharges = 0,
                Deposit = -1,
                LeaseStartDate = DateTime.MinValue,
                LeaseEndDate = DateTime.MaxValue,
                PropertyStatus = PropertyStatus.Available,
                Surface = 0,
                NumberRooms = 0,
                Tenants = null!,
                Address = null!,
                Documents = null!
            };
            _propertyQueriesMock.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ReturnsAsync(expectedProperty);
            var query = new DetailPropertyQuery(propertyId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedProperty);
        }
    }
}
