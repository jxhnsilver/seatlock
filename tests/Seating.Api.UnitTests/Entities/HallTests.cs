using FluentAssertions;
using Seating.Api.Domain.Halls;
using Seating.Api.Entities;

namespace Seating.Api.UnitTests.Entities
{
    public sealed class HallTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            string expectedName = "Красный зал";
            var expectedType = HallType.Standard;

            var hall = new Hall(expectedName, expectedType);

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
            var type = HallType.Standard;

            Action act = () => _ = new Hall(invalidName, type);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Название зала не может быть пустым.*")
                .And.ParamName.Should().Be("name"); ;
        }

        [Fact]
        public void SetName_WithValidName_ShouldUpdateName()
        {
            var hall = new Hall("Красный зал", HallType.Standard);
            string newName = "Синий зал";

            hall.SetName(newName);

            hall.Name.Should().Be(newName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SetName_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            var hall = new Hall("Красный зал", HallType.Standard);

            Action act = () => hall.SetName(invalidName);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Название зала не может быть пустым.*")
                .And.ParamName.Should().Be("newName"); ;
        }

        [Fact]
        public void ChangeStatus_WithNewStatus_ShouldUpdateStatus()
        {
            var hall = new Hall("Синий зал", HallType.Standard);
            var newStatus = HallStatus.Maintenance;

            hall.ChangeStatus(newStatus);

            hall.Status.Should().Be(newStatus);
        }
    }
}
