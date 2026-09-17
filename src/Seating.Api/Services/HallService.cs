using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;
using Seating.Api.Domain.Halls;
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

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _db.Halls
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
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
    }
}
