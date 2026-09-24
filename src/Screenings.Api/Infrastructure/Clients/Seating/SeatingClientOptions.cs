namespace Screenings.Api.Infrastructure.Clients.Seating
{
    public sealed class SeatingClientOptions
    {
        public required string BaseUrl { get; init; }

        public int TimeoutSeconds { get; init; }
    }
}