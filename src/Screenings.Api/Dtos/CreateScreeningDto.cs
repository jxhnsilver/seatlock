namespace Screenings.Api.Dtos
{
    public sealed record CreateScreeningDto(
        int MovieId,
        int HallId,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}
