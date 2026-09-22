namespace Seating.Api.Dtos
{
    public sealed record HallLayoutDto(
        int HallId,
        string HallName,
        IReadOnlyList<LayoutSeatDto> Seats
        );
}
