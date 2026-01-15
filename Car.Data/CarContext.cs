namespace Car.Data
{
    public class CarContext : IdentityDbContext<ApplicationUser>
    {
        public CarContext(DbContextOptions<CarContext> options)
        : base(options) { }
        public DbSet<Car> Cars { get; set; }
    }
}
