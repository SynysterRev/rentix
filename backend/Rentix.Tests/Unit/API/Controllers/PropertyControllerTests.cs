using Xunit;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Rentix.API.Controllers.v1;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Queries.Detail;
using Rentix.Application.RealEstate.Queries.List;
using Rentix.Domain.Entities;

namespace Rentix.Tests.Unit.API.Controllers
{
    public class PropertyControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PropertyController>> _loggerMock;
        private readonly PropertyController _controller;

        public PropertyControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PropertyController>>();
            _controller = new PropertyController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProperties_ReturnsOk_WithPropertyList()
        {
            // Arrange
            var properties = new List<PropertyListDto>
            {
                new PropertyListDto(
                    1,
                    "Test",
                    1000m,
                    new List<string> { "Tenant1" },
                    PropertyStatus.Available,
                    null!
                )
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ListPropertiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(properties);

            // Act
            var result = await _controller.GetProperties();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(properties, okResult.Value);
        }

        [Fact]
        public async Task GetProperties_ReturnsOk_WithEmptyList()
        {
            // Arrange
            var properties = new List<PropertyListDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ListPropertiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(properties);

            // Act
            var result = await _controller.GetProperties();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(properties, okResult.Value);
        }

        [Fact]
        public async Task GetPropertyDetail_ReturnsOk_WithPropertyDetail()
        {
            // Arrange
            var propertyDetail = new PropertyDetailDto { Id = 1, Name = "Test Detail" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<DetailPropertyQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(propertyDetail);

            // Act
            var result = await _controller.GetPropertyDetail(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(propertyDetail, okResult.Value);
        }

        [Fact]
        public async Task GetPropertyDetail_ReturnsOk_WithNull_WhenNotFound()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DetailPropertyQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PropertyDetailDto)null);

            // Act
            var result = await _controller.GetPropertyDetail(99);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Null(okResult.Value);
        }
    }
}
