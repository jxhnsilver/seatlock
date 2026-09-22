using BuildingBlocks.Exceptions;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;
using Seating.Api.Domain.Halls;
using Seating.Api.Domain.Seats;
using Seating.Api.Dtos;
using Seating.Api.Services;

namespace Seating.Api.UnitTests.Services
{
    public sealed class HallServiceTests
    {
        private static readonly string[] ExpectedHallNames = ["A", "B"];

        private static SeatingDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<SeatingDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new SeatingDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistAndReturnId()
        {
            // Arrange
            await using var db = CreateContext();
            var service = new HallService(db);

            var dto = new CreateHallDto("Test Hall", HallType.Standard);

            // Act
            var id = await service.CreateAsync(dto, TestContext.Current.CancellationToken);

            // Assert
            id.Should().BeGreaterThan(0);
            var persisted = await db.Halls.FindAsync([id], TestContext.Current.CancellationToken);
            persisted.Should().NotBeNull();
            persisted!.Name.Should().Be(dto.Name);
            persisted.Type.Should().Be(dto.Type);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateHallDetails()
        {
            await using var db = CreateContext();
            var hall = new Hall("Old Hall", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);
            var dto = new UpdateHallDto("New Hall", HallType.Imax);

            await service.UpdateAsync(hall.Id, dto, TestContext.Current.CancellationToken);

            var updated = await db.Halls.FindAsync([hall.Id], TestContext.Current.CancellationToken);
            updated!.Name.Should().Be(dto.Name);
            updated.Type.Should().Be(dto.Type);
            updated.Status.Should().Be(HallStatus.Active);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHalls()
        {
            // Arrange
            await using var db = CreateContext();
            db.Halls.AddRange(
                new Hall(ExpectedHallNames[0], HallType.Standard),
                new Hall(ExpectedHallNames[1], HallType.Imax)
            );
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            // Act
            var result = await service.GetAllAsync(TestContext.Current.CancellationToken);

            // Assert
            result.Should().HaveCount(2);
            result.Select(r => r.Name).Should().Contain(ExpectedHallNames);
        }

        [Fact]
        public async Task GetActiveAsync_ShouldReturnOnlyActiveHalls()
        {
            await using var db = CreateContext();
            var activeHall = new Hall("Active Hall", HallType.Standard);
            var maintenanceHall = new Hall("Maintenance Hall", HallType.Standard);
            maintenanceHall.ChangeStatus(HallStatus.Maintenance);
            var inactiveHall = new Hall("Inactive Hall", HallType.Standard);
            inactiveHall.ChangeStatus(HallStatus.Inactive);

            db.Halls.AddRange(activeHall, maintenanceHall, inactiveHall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            var result = await service.GetActiveAsync(TestContext.Current.CancellationToken);

            result.Should().ContainSingle();
            result[0].Id.Should().Be(activeHall.Id);
            result[0].Status.Should().Be(HallStatus.Active);
        }

        [Fact]
        public async Task GetByIdAsync_WhenExists_ShouldReturnDto()
        {
            // Arrange
            await using var db = CreateContext();
            var hall = new Hall("Single", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            // Act
            var dto = await service.GetByIdAsync(hall.Id, TestContext.Current.CancellationToken);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(hall.Id);
            dto.Name.Should().Be(hall.Name);
            dto.Type.Should().Be(hall.Type);
            dto.Status.Should().Be(hall.Status);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            await using var db = CreateContext();
            var service = new HallService(db);

            // Act
            var act = async () => await service.GetByIdAsync(999, TestContext.Current.CancellationToken);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldUpdateHallStatus()
        {
            await using var db = CreateContext();
            var hall = new Hall("Hall", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            await service.ChangeStatusAsync(
                hall.Id,
                new ChangeHallStatusDto(HallStatus.Inactive),
                TestContext.Current.CancellationToken);

            var updated = await db.Halls.FindAsync([hall.Id], TestContext.Current.CancellationToken);
            updated!.Status.Should().Be(HallStatus.Inactive);
        }

        [Fact]
        public async Task GetLayoutAsync_WhenExists_ShouldReturnHallLayout()
        {
            // Arrange
            await using var db = CreateContext();
            var hall = new Hall("Layout Hall", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            db.Seats.AddRange(
                new Seat(hall.Id, 2, 1, SeatType.Double),
                new Seat(hall.Id, 1, 3, SeatType.Single)
            );
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            // Act
            var layout = await service.GetLayoutAsync(hall.Id, TestContext.Current.CancellationToken);

            // Assert
            layout.HallId.Should().Be(hall.Id);
            layout.HallName.Should().Be(hall.Name);
            layout.Seats.Should().HaveCount(2);
            layout.Seats[0].RowNumber.Should().Be(1);
            layout.Seats[0].SeatNumber.Should().Be(3);
            layout.Seats[0].Type.Should().Be(SeatType.Single);
            layout.Seats[1].RowNumber.Should().Be(2);
            layout.Seats[1].SeatNumber.Should().Be(1);
            layout.Seats[1].Type.Should().Be(SeatType.Double);
        }

        [Fact]
        public async Task GetLayoutAsync_WhenHallNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            await using var db = CreateContext();
            var service = new HallService(db);

            // Act
            var act = async () => await service.GetLayoutAsync(999, TestContext.Current.CancellationToken);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SetLayoutAsync_ShouldReplaceExistingSeats()
        {
            // Arrange
            await using var db = CreateContext();
            var hall = new Hall("Layout Hall", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            db.Seats.AddRange(
                new Seat(hall.Id, 1, 1, SeatType.Single),
                new Seat(hall.Id, 1, 2, SeatType.Single)
            );
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            var newLayout = new CreateSeatLayoutDto(new List<CreateSeatDto>
            {
                new(2, 1, SeatType.Double),
                new(2, 2, SeatType.Single),
                new(3, 1, SeatType.Single)
            });

            // Act
            await service.SetLayoutAsync(hall.Id, newLayout, TestContext.Current.CancellationToken);

            // Assert
            var seats = await db.Seats
                .AsNoTracking()
                .Where(s => s.HallId == hall.Id)
                .OrderBy(s => s.RowNumber)
                .ThenBy(s => s.SeatNumber)
                .ToListAsync(TestContext.Current.CancellationToken);

            seats.Should().HaveCount(3);
            seats.Select(s => s.RowNumber).Should().ContainInOrder(2, 2, 3);
            seats.Select(s => s.SeatNumber).Should().ContainInOrder(1, 2, 1);
            seats.Select(s => s.Type).Should().ContainInOrder(SeatType.Double, SeatType.Single, SeatType.Single);

            var oldSeatsExist = await db.Seats.AnyAsync(
                s => s.HallId == hall.Id && s.RowNumber == 1,
                TestContext.Current.CancellationToken);

            oldSeatsExist.Should().BeFalse();
        }

        [Fact]
        public async Task SetLayoutAsync_WhenHallNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            await using var db = CreateContext();
            var service = new HallService(db);

            var newLayout = new CreateSeatLayoutDto(new List<CreateSeatDto>
            {
                new(1, 1, SeatType.Single)
            });

            // Act
            var act = async () => await service.SetLayoutAsync(999, newLayout, TestContext.Current.CancellationToken);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SetLayoutAsync_WhenLayoutContainsDuplicateSeat_ShouldThrowBusinessRuleException()
        {
            await using var db = CreateContext();
            var service = new HallService(db);
            var layout = new CreateSeatLayoutDto(
            [
                new CreateSeatDto(1, 1, SeatType.Single),
                new CreateSeatDto(1, 1, SeatType.Double)
            ]);

            var act = () => service.SetLayoutAsync(999, layout, TestContext.Current.CancellationToken);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }
    }
}
