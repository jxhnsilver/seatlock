using Seating.Api.Dtos;

namespace Seating.Api.Services
{
    public interface IHallService
    {
        Task<int> CreateAsync(CreateHallDto createHallDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, UpdateHallDto updateHallDto, CancellationToken cancellationToken = default);
        Task ChangeStatusAsync(int id, ChangeHallStatusDto changeStatusDto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<HallDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<HallDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<HallLayoutDto> GetLayoutAsync(int hallId, CancellationToken cancellationToken = default);
        Task SetLayoutAsync(int hallId, CreateSeatLayoutDto newSeatsDto, CancellationToken cancellationToken = default);
    }
}
