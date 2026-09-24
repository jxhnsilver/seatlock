using BuildingBlocks.Exceptions;

namespace Seating.Api.Domain.Seats
{
    /// <summary>
    /// Посадочное место в кинозале.
    /// </summary>
    public sealed class Seat
    {
        /// <summary>
        /// Уникальный идентификатор места.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Идентификатор зала, к которому относится место.
        /// </summary>
        public int HallId { get; init; }

        /// <summary>
        /// Номер ряда.
        /// </summary>
        public int RowNumber { get; init; }

        /// <summary>
        /// Номер места в ряду.
        /// </summary>
        public int SeatNumber { get; init; }

        /// <summary>
        /// Тип посадочного места.
        /// </summary>
        public SeatType Type { get; init; }

        public Seat(int hallId, int rowNumber, int seatNumber, SeatType type)
        {
            if (hallId <= 0)
                throw new InvariantViolationException("Идентификатор зала должен быть больше нуля.");

            if (rowNumber <= 0)
                throw new InvariantViolationException("Номер ряда должен быть больше нуля.");

            if (seatNumber <= 0)
                throw new InvariantViolationException("Номер места должен быть больше нуля.");

            HallId = hallId;
            RowNumber = rowNumber;
            SeatNumber = seatNumber;
            Type = type;
        }
    }
}
