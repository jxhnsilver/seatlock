using Seating.Api.Dtos;

namespace Seating.Api.Services
{
    public interface IHallService
    {
        Task<int> CreateAsync(CreateHallDto createHallDto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<HallDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<HallDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
