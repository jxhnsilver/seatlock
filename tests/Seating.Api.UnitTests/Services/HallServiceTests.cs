using BuildingBlocks.Exceptions;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;
using Seating.Api.Domain.Halls;
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
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            await using var db = CreateContext();
            var hall = new Hall("ToDelete", HallType.Standard);
            db.Halls.Add(hall);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new HallService(db);

            // Act
            await service.DeleteAsync(hall.Id, TestContext.Current.CancellationToken);

            // Assert
            var exists = await db.Halls.AnyAsync(h => h.Id == hall.Id, TestContext.Current.CancellationToken);
            exists.Should().BeFalse();
        }
    }
}
