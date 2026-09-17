using Seating.Api.Domain.Halls;

namespace Seating.Api.Dtos
{
    public sealed record CreateHallDto(string Name, HallType Type);
}
