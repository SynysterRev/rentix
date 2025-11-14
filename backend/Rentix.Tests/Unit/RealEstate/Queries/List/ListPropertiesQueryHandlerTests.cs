using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Queries.List;
using Rentix.Domain.Entities;
using Xunit;
using FluentAssertions;
using Rentix.Application.Common.Interfaces.Queries;

namespace Rentix.Tests.Unit.RealEstate.Queries.List
{
    public class ListPropertiesQueryHandlerTests
    {
        private readonly Mock<IPropertyQueries> _propertyQueriesMock;
        private readonly ListPropertiesQueryHandler _handler;

        public ListPropertiesQueryHandlerTests()
        {
            _propertyQueriesMock = new Mock<IPropertyQueries>();
            _handler = new ListPropertiesQueryHandler(_propertyQueriesMock.Object);
        }

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
            _propertyQueriesMock.Setup(x => x.GetPropertyListAsync())
                .ReturnsAsync(expectedList);
            var query = new ListPropertiesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPropertiesExist()
        {
            // Arrange
            _propertyQueriesMock.Setup(x => x.GetPropertyListAsync())
                .ReturnsAsync(new List<PropertyListDto>());
            var query = new ListPropertiesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            _propertyQueriesMock.Setup(x => x.GetPropertyListAsync())
                .ThrowsAsync(new System.Exception("Repository error"));
            var query = new ListPropertiesQuery();

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
