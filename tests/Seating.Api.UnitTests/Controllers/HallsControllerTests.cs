using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Seating.Api.Controllers;
using Seating.Api.Domain.Halls;
using Seating.Api.Domain.Seats;
using Seating.Api.Dtos;
using Seating.Api.Services;

namespace Seating.Api.UnitTests.Controllers
{
    public sealed class HallsControllerTests
    {
        private readonly Mock<IHallService> _serviceMock;
        private readonly HallsController _controller;

        public HallsControllerTests()
        {
            _serviceMock = new Mock<IHallService>(MockBehavior.Strict);
            _controller = new HallsController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithHallDto()
        {
            // Arrange
            var hallDto = new HallDto(1, "Hall 1", HallType.Standard, HallStatus.Active);
            _serviceMock.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hallDto);

            // Act
            var result = await _controller.GetById(1, TestContext.Current.CancellationToken);

            // Assert
            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(hallDto);
            _serviceMock.Verify(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithHalls()
        {
            // Arrange
            var halls = new List<HallDto>
            {
                new(1, "A", HallType.Standard, HallStatus.Active),
                new(2, "B", HallType.Imax, HallStatus.Active)
            };

            _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(halls);

            // Act
            var result = await _controller.GetAll(TestContext.Current.CancellationToken);

            // Assert
            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(halls);
            _serviceMock.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetActive_ShouldReturnOkWithActiveHalls()
        {
            var halls = new List<HallDto>
            {
                new(1, "Active Hall", HallType.Standard, HallStatus.Active)
            };

            _serviceMock.Setup(s => s.GetActiveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(halls);

            var result = await _controller.GetActive(TestContext.Current.CancellationToken);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(halls);
            _serviceMock.Verify(s => s.GetActiveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWithId()
        {
            // Arrange
            var dto = new CreateHallDto("New Hall", HallType.Standard);
            _serviceMock.Setup(s => s.CreateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(42);

            // Act
            var result = await _controller.Create(dto, TestContext.Current.CancellationToken);

            // Assert
            var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            created.ActionName.Should().Be(nameof(HallsController.GetById));
            created.RouteValues!["id"].Should().Be(42);
            created.Value.Should().Be(42);

            _serviceMock.Verify(s => s.CreateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            var dto = new UpdateHallDto("Updated Hall", HallType.Imax);
            _serviceMock.Setup(s => s.UpdateAsync(10, dto, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(10, dto, TestContext.Current.CancellationToken);

            result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.UpdateAsync(10, dto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_ShouldReturnNoContent()
        {
            var dto = new ChangeHallStatusDto(HallStatus.Inactive);
            _serviceMock.Setup(s => s.ChangeStatusAsync(10, dto, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.ChangeStatus(10, dto, TestContext.Current.CancellationToken);

            result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.ChangeStatusAsync(10, dto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLayout_ShouldReturnOkWithLayout()
        {
            // Arrange
            var layout = new HallLayoutDto(
                1,
                "Hall 1",
                [
                    new LayoutSeatDto(1, 1, 1, SeatType.Single),
                    new LayoutSeatDto(2, 1, 2, SeatType.Double)
                ]);

            _serviceMock.Setup(s => s.GetLayoutAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(layout);

            // Act
            var result = await _controller.GetLayout(1, TestContext.Current.CancellationToken);

            // Assert
            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(layout);
            _serviceMock.Verify(s => s.GetLayoutAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SetLayout_ShouldReturnNoContent()
        {
            // Arrange
            var dto = new CreateSeatLayoutDto(
                [
                    new CreateSeatDto(1, 1, SeatType.Single),
                    new CreateSeatDto(1, 2, SeatType.Double)
                ]);

            _serviceMock.Setup(s => s.SetLayoutAsync(5, dto, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SetLayout(5, dto, TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.SetLayoutAsync(5, dto, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
