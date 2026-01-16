using Car.Core.Dto;
using Car.Core.ServiceInterface;
using Car.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DomainCar = global::Car.Core.Domain.Car;

namespace Car.CarTest
{
    public class CarServicesTests
    {
        private static ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();

            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<CarContext>(o => o.UseInMemoryDatabase(dbName));

            services.AddScoped<ICarServices, global::Car.ApplicationServices.Services.CarServices>();

            return services.BuildServiceProvider(validateScopes: true);
        }

        [Fact]
        public async Task Create_PersistsEntity()
        {
            using var provider = BuildProvider();
            using var scope = provider.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<CarContext>();
            var service = scope.ServiceProvider.GetRequiredService<ICarServices>();

            var dto = new CarDto
            {
                Id = Guid.Empty,
                Make = "Toyota",
                Model = "Camry",
                Year = 2020,
                Mileage = 10000,
                CreatedAt = null,
                ModifiedAt = null
            };

            var created = await service.Create(dto);

            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);

            var exists = await ctx.Cars.AnyAsync(x => x.Id == created.Id);
            Assert.True(exists);
        }

        [Fact]
        public async Task DetailAsync_ReturnsExistingEntity()
        {
            using var provider = BuildProvider();
            using var scope = provider.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<CarContext>();
            var service = scope.ServiceProvider.GetRequiredService<ICarServices>();

            var id = Guid.NewGuid();
            ctx.Cars.Add(new DomainCar
            {
                Id = id,
                Make = "BMW",
                Model = "X5",
                Year = 2019,
                Mileage = 50000,
                CreatedAt = null,
                ModifiedAt = null
            });
            await ctx.SaveChangesAsync();

            var result = await service.DetailAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("BMW", result.Make);
            Assert.Equal("X5", result.Model);
        }

        [Fact]
        public async Task Update_ModifiesEntity()
        {
            using var provider = BuildProvider();

            var id = Guid.NewGuid();

            using (var scope = provider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<CarContext>();
                ctx.Cars.Add(new DomainCar
                {
                    Id = id,
                    Make = "Audi",
                    Model = "A4",
                    Year = 2018,
                    Mileage = 70000,
                    CreatedAt = null,
                    ModifiedAt = null
                });
                await ctx.SaveChangesAsync();
            }

            var dto = new CarDto
            {
                Id = id,
                Make = "Audi",
                Model = "A6",
                Year = 2018,
                Mileage = 71000,
                CreatedAt = null,
                ModifiedAt = DateTime.UtcNow
            };

            DomainCar updated;
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICarServices>();
                updated = await service.Update(dto);
            }

            Assert.NotNull(updated);
            Assert.Equal(id, updated.Id);
            Assert.Equal("A6", updated.Model);
            Assert.Equal(71000, updated.Mileage);

            using (var scope = provider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<CarContext>();
                var inDb = await ctx.Cars.SingleAsync(x => x.Id == id);
                Assert.Equal("A6", inDb.Model);
                Assert.Equal(71000, inDb.Mileage);
            }
        }

        [Fact]
        public async Task Delete_RemovesEntity()
        {
            using var provider = BuildProvider();
            using var scope = provider.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<CarContext>();
            var service = scope.ServiceProvider.GetRequiredService<ICarServices>();

            var id = Guid.NewGuid();
            ctx.Cars.Add(new DomainCar
            {
                Id = id,
                Make = "Ford",
                Model = "Focus",
                Year = 2016,
                Mileage = 90000,
                CreatedAt = null,
                ModifiedAt = null
            });
            await ctx.SaveChangesAsync();

            var deleted = await service.Delete(id);

            Assert.NotNull(deleted);
            Assert.Equal(id, deleted.Id);

            var exists = await ctx.Cars.AnyAsync(x => x.Id == id);
            Assert.False(exists);
        }
    }
}