namespace Seating.Api.Dtos
{
    public sealed record CreateSeatLayoutDto(
        List<CreateSeatDto> Seats
        );
}
