using Seating.Api.Domain.Halls;

namespace Seating.Api.Dtos
{
    public sealed record UpdateHallDto(string Name, HallType Type);
}