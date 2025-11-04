using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Queries.List;
using Rentix.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace Rentix.Tests.Unit.RealEstate.Queries.List
{
    public class ListPropertiesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfProperties()
        {
            // Arrange
            var expectedList = new List<PropertyListDto>
            {
                new PropertyListDto(
                    1,
                    "Property One",
                    1200,
                    new List<string> { "Alice" },
                    PropertyStatus.Available,
                    null!
                ),
                new PropertyListDto(
                    2,
                    "Property Two",
                    1500,
                    new List<string> { "Bob", "Charlie" },
                    PropertyStatus.Rented,
                    null!
                )
            };
            var propertyQueriesMock = new Mock<IPropertyQueries>();
            propertyQueriesMock.Setup(x => x.GetPropertyListAsync())
                .ReturnsAsync(expectedList);
            var handler = new ListPropertiesQueryHandler(propertyQueriesMock.Object);
            var query = new ListPropertiesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }
    }
}
