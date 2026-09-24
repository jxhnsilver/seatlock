using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Screenings.Api.Data;
using Screenings.Api.Domain;
using Screenings.Api.Dtos;
using Screenings.Api.Infrastructure.Clients.Seating;

namespace Screenings.Api.Services
{
    public sealed class ScreeningService : IScreeningService
    {
        private readonly ScreeningDbContext _db;
        private readonly ISeatingClient _seatingClient;

        public ScreeningService(ScreeningDbContext db, ISeatingClient seatingClient)
        {
            _db = db;
            _seatingClient = seatingClient;
        }

        public async Task<int> CreateAsync(
            CreateScreeningDto dto,
            CancellationToken cancellationToken = default)
        {
            var hall = await _seatingClient.GetHallAsync(dto.HallId, cancellationToken);

            if (!hall.IsActive)
                throw new BusinessRuleException(
                    $"Hall with id {dto.HallId} is not available for screening.");

            var hasOverlap = await _db.Screenings
                .AnyAsync(screening =>
                    screening.HallId == dto.HallId &&
                    screening.Status != ScreeningStatus.Completed &&
                    dto.StartTime < screening.EndTime &&
                    dto.EndTime > screening.StartTime,
                    cancellationToken);

            if (hasOverlap)
                throw new BusinessRuleException(
                    $"Hall with id {dto.HallId} already has an overlapping screening.");

            var screening = new Screening(
                dto.MovieId,
                hall.Id,
                dto.StartTime,
                dto.EndTime);

            _db.Screenings.Add(screening);
            await _db.SaveChangesAsync(cancellationToken);

            return screening.Id;
        }

    }
}
