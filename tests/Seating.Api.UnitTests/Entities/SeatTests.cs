using FluentAssertions;
using Seating.Api.Domain.Seats;
using Seating.Api.Entities;

namespace Seating.Api.UnitTests.Entities
{
    public sealed class SeatTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            int expectedHallId = 1;
            int expectedRowNumber = 5;
            int expectedSeatNumber = 12;
            var expectedType = SeatType.Single;

            var seat = new Seat(expectedHallId, expectedRowNumber, expectedSeatNumber, expectedType);

            seat.HallId.Should().Be(expectedHallId);
            seat.RowNumber.Should().Be(expectedRowNumber);
            seat.SeatNumber.Should().Be(expectedSeatNumber);
            seat.Type.Should().Be(expectedType);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidHallId_ShouldThrowArgumentException(int invalidHallId)
        {
            int rowNumber = 5;
            int seatNumber = 12;
            var type = SeatType.Single;

            Action act = () => _ = new Seat(invalidHallId, rowNumber, seatNumber, type);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Идентификатор зала должен быть больше нуля.*")
                .And.ParamName.Should().Be("hallId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidRowNumber_ShouldThrowArgumentException(int invalidRowNumber)
        {
            int hallId = 1;
            int seatNumber = 12;
            var type = SeatType.Single;

            Action act = () => _ = new Seat(hallId, invalidRowNumber, seatNumber, type);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Номер ряда должен быть больше нуля.*")
                .And.ParamName.Should().Be("rowNumber");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidSeatNumber_ShouldThrowArgumentException(int invalidSeatNumber)
        {
            int hallId = 1;
            int rowNumber = 5;
            var type = SeatType.Single;

            Action act = () => _ = new Seat(hallId, rowNumber, invalidSeatNumber, type);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Номер места должен быть больше нуля.*")
                .And.ParamName.Should().Be("seatNumber");
        }
    }
}
