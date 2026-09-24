namespace Screenings.Api.Infrastructure.Clients.Seating
{
    public interface ISeatingClient
    {
        Task<HallInfo> GetHallAsync(
            int hallId,
            CancellationToken cancellationToken = default);
    }
}