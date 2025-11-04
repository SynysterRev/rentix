using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.DTOs.Tenants;
using Rentix.Application.RealEstate.DTOs.Documents;
using Rentix.Application.RealEstate.Queries.Detail;
using Rentix.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace Rentix.Tests.Unit.RealEstate.Queries.Detail
{
    public class DetailPropertyQueryHandlerTests
    {
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
            var propertyQueriesMock = new Mock<IPropertyQueries>();
            propertyQueriesMock.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ReturnsAsync(expectedProperty);
            var handler = new DetailPropertyQueryHandler(propertyQueriesMock.Object);
            var query = new DetailPropertyQuery(propertyId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedProperty);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 2;
            var propertyQueriesMock = new Mock<IPropertyQueries>();
            propertyQueriesMock.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ReturnsAsync((PropertyDetailDto?)null);
            var handler = new DetailPropertyQueryHandler(propertyQueriesMock.Object);
            var query = new DetailPropertyQuery(propertyId);

            // Act & Assert
            await handler.Invoking(h => h.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Property with ID {propertyId} does not exist");
        }
    }
}
