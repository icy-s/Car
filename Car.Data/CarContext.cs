using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Car.Core.Domain;
using DomainCar = global::Car.Core.Domain.Car;

namespace Car.Data
{
    public class CarContext : IdentityDbContext<ApplicationUser>
    {
        public CarContext(DbContextOptions<CarContext> options)
        : base(options) { }
        public DbSet<DomainCar> Cars { get; set; }
    }
}
