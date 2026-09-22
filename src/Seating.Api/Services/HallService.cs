using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;
using Seating.Api.Domain.Halls;
using Seating.Api.Domain.Seats;
using Seating.Api.Dtos;

namespace Seating.Api.Services
{
    public sealed class HallService : IHallService
    {
        private readonly SeatingDbContext _db;

        public HallService(SeatingDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(CreateHallDto createHallDto, CancellationToken cancellationToken = default)
        {
            var hall = new Hall(createHallDto.Name, createHallDto.Type);

            _db.Halls.Add(hall);
            await _db.SaveChangesAsync(cancellationToken);

            return hall.Id;
        }

        public async Task UpdateAsync(
            int id,
            UpdateHallDto updateHallDto,
            CancellationToken cancellationToken = default)
        {
            var hall = await _db.Halls
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Hall with id {id} not found.");

            hall.SetName(updateHallDto.Name);
            hall.SetType(updateHallDto.Type);

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeStatusAsync(
            int id,
            ChangeHallStatusDto changeStatusDto,
            CancellationToken cancellationToken = default)
        {
            var hall = await _db.Halls
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Hall with id {id} not found.");

            hall.ChangeStatus(changeStatusDto.Status);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<HallDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var halls = await _db.Halls
                .AsNoTracking()
                .OrderBy(h => h.Id)
                .ToListAsync(cancellationToken);

            return halls.Select(h => new HallDto(
                h.Id, 
                h.Name, 
                h.Type, 
                h.Status)
            ).ToList();
        }

        public async Task<HallDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var hall = await _db.Halls
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Hall with id {id} not found.");

            return new HallDto(hall.Id, hall.Name, hall.Type, hall.Status);
        }

        public async Task<HallLayoutDto> GetLayoutAsync(int hallId, CancellationToken cancellationToken = default)
        {
            var hall = await _db.Halls
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == hallId, cancellationToken)
                ?? throw new NotFoundException($"Hall with id {hallId} not found.");

            var seats = await _db.Seats
                .AsNoTracking()
                .Where(s => s.HallId == hall.Id)
                .OrderBy(s => s.RowNumber)
                .ToListAsync(cancellationToken);

            var seatDtos = seats.Select(s => new LayoutSeatDto(
                s.Id, 
                s.RowNumber, 
                s.SeatNumber, 
                s.Type)
            ).ToList();

            return new HallLayoutDto(hall.Id, hall.Name, seatDtos);
        }

        public async Task SetLayoutAsync(int hallId, CreateSeatLayoutDto newSeatsDto, CancellationToken cancellationToken = default)
        {
            var duplicateSeat = newSeatsDto.Seats
                .GroupBy(s => new { s.RowNumber, s.SeatNumber })
                .FirstOrDefault(group => group.Count() > 1);

            if (duplicateSeat is not null)
            {
                throw new BusinessRuleException(
                    $"Seat at row {duplicateSeat.Key.RowNumber}, number {duplicateSeat.Key.SeatNumber} is specified more than once.");
            }

            var hall = await _db.Halls
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == hallId, cancellationToken)
                ?? throw new NotFoundException($"Hall with id {hallId} not found.");

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var oldSeats = await _db.Seats
                    .Where(s => s.HallId == hallId)
                    .ExecuteDeleteAsync(cancellationToken);

                var seats = newSeatsDto.Seats.Select(s => new Seat(
                        hallId, 
                        s.RowNumber, 
                        s.SeatNumber, 
                        s.Type)
                ).ToList();

                _db.AddRange(seats);
                await _db.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
