using Seating.Api.Domain.Seats;

namespace Seating.Api.Dtos
{
    public sealed record LayoutSeatDto(
        int Id,
        int RowNumber,
        int SeatNumber,
        SeatType Type
        );
}