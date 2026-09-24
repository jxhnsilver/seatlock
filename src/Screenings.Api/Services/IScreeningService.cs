using Screenings.Api.Dtos;

namespace Screenings.Api.Services
{
    public interface IScreeningService
    {
        Task<int> CreateAsync(
            CreateScreeningDto dto,
            CancellationToken cancellationToken = default);
    }
}
