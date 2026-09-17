using FluentAssertions;
using Seating.Api.Domain.Halls;

namespace Seating.Api.UnitTests.Domain
{
    public sealed class HallTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange
            string expectedName = "Красный зал";
            var expectedType = HallType.Standard;

            // Act
            var hall = new Hall(expectedName, expectedType);

            // Assert
            hall.Name.Should().Be(expectedName);
            hall.Type.Should().Be(expectedType);
            hall.Status.Should().Be(HallStatus.Active);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var type = HallType.Standard;

            // Act
            Action act = () => _ = new Hall(invalidName, type);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Название зала не может быть пустым.*")
                .And.ParamName.Should().Be("name"); ;
        }

        [Fact]
        public void SetName_WithValidName_ShouldUpdateName()
        {
            // Arrange
            var hall = new Hall("Красный зал", HallType.Standard);
            string newName = "Синий зал";

            // Act
            hall.SetName(newName);

            // Assert
            hall.Name.Should().Be(newName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SetName_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var hall = new Hall("Красный зал", HallType.Standard);

            // Act
            Action act = () => hall.SetName(invalidName);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Название зала не может быть пустым.*")
                .And.ParamName.Should().Be("newName"); ;
        }

        [Fact]
        public void ChangeStatus_WithNewStatus_ShouldUpdateStatus()
        {
            // Arrange
            var hall = new Hall("Синий зал", HallType.Standard);
            var newStatus = HallStatus.Maintenance;

            // Act
            hall.ChangeStatus(newStatus);

            // Assert
            hall.Status.Should().Be(newStatus);
        }
    }
}
