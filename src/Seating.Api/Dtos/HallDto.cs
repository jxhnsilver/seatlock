using Seating.Api.Domain.Halls;

namespace Seating.Api.Dtos
{
    public sealed record HallDto(int Id, string Name, HallType Type, HallStatus Status);
}
