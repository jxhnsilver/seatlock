using BuildingBlocks.Exceptions;

namespace Screenings.Api.Domain
{
    public sealed class Screening
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public ScreeningStatus Status { get; set; }

        public Screening(int movieId, int hallId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            if (movieId <= 0)
                throw new InvariantViolationException("MovieId is required.");

            if (hallId <= 0)
                throw new InvariantViolationException("HallId must be greater than zero.");

            if (endTime <= startTime)
                throw new InvariantViolationException(
                    "Screening end must be later than its start.");

            MovieId = movieId;
            HallId = hallId;
            StartTime = startTime;
            EndTime = endTime;
            Status = ScreeningStatus.Scheduled;
        }
    }
}
