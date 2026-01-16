using Microsoft.EntityFrameworkCore;
using Car.Core.Domain;
using Car.Core.Dto;
using Car.Data;

namespace Car.ApplicationServices.Services
{
    public class CarServices : ICarServices
    {
        private readonly CarContext _context;
        public CarServices
            (
                CarContext context
            )
        {
            _context = context;
        }
        public async Task<Car> Create(CarDto dto)
        {
            Car car = new Car();

            car.Id = Guid.NewGuid();
            car.Make = dto.Make;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.Mileage = dto.Mileage;
            car.CreatedAt = DateTime.Now;
            car.ModifiedAt = DateTime.Now;

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car> DetailAsync(Guid id)
        {
            var result = await _context.Cars
                .FirstOrDefaultAsync( x  => x.Id == id );

            return result;
        }

        public async Task <Car> Delete (Guid id)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(x  => x.Id == id);

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car> Update (CarDto dto)
        {
            Car domain = new();

            domain.Id = dto.Id;
            domain.Make = dto.Make;
            domain.Model = dto.Model;
            domain.Year = dto.Year;
            domain.Mileage = dto.Mileage;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            _context.Cars.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
