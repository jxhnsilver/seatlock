using Seating.Api.Domain.Seats;

namespace Seating.Api.Dtos
{
    public sealed record CreateSeatDto(
        int RowNumber,
        int SeatNumber,
        SeatType Type
        );
}
