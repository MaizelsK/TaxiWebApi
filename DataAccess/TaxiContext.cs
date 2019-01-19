using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace DataAccess
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MessageDb;Trusted_Connection=True;");
        }
    }
}
