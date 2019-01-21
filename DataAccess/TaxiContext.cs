using BCrypt;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaxiDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User client = new User
            {
                Id = 1,
                Username = "petya",
                Email = "petya@mail.ru",
                PasswordHash = BCryptHelper.HashPassword("1234", BCryptHelper.GenerateSalt()),
                Role = "client"
            };

            User driver = new User
            {
                Id = 2,
                Username = "driver",
                Email = "driver@mail.ru",
                PasswordHash = BCryptHelper.HashPassword("1234", BCryptHelper.GenerateSalt()),
                Role = "driver"
            };

            Order order = new Order
            {
                Id = 1,
                StartPoint = "Astana",
                EndPoint = "Almaty",
                Price = 10000,
                Status = OrderStatus.Created,
                UserId = 1
            };

            modelBuilder.Entity<User>().HasData(client, driver);
            modelBuilder.Entity<Order>().HasData(order);

            base.OnModelCreating(modelBuilder);
        }
    }
}
