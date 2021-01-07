using Microsoft.EntityFrameworkCore;
using Truck_Registration_Control.Domain;

namespace Truck_Registration_Control.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        
        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TruckModel> TruckModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Truck>().HasKey(x => x.Id);
            modelBuilder.Entity<TruckModel>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}