using BuildingBlocks.Exceptions;
using System.Net;

namespace Screenings.Api.Infrastructure.Clients.Seating
{
    public sealed partial class SeatingClient : ISeatingClient
    {
        private readonly HttpClient _httpClient;

        public SeatingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HallInfo> GetHallAsync(
            int hallId,
            CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(
                $"api/halls/{hallId}",
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException(
                    $"Hall with id {hallId} was not found.");
            }

            response.EnsureSuccessStatusCode();

            var hall = await response.Content
                .ReadFromJsonAsync<HallResponse>(cancellationToken);

            return hall is null
                ? throw new InvalidOperationException(
                    "Seating.Api returned an empty hall response.")
                : new HallInfo(
                    hall.Id,
                    string.Equals(hall.Status, "Active", StringComparison.OrdinalIgnoreCase));
        }
        private sealed record HallResponse(
            int Id,
            string Name,
            string Type,
            string Status);
    }

}