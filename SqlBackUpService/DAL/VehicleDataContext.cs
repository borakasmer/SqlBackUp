using Microsoft.EntityFrameworkCore;

public class VehicleDataContext : DbContext
  {
    public DbSet<Vehicle> Vehicle { get; set; }

    public DbSet<VehicleBackUp> VehicleBackUp { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      
      optionsBuilder.UseSqlServer("Server=tcp:10.211.55.9,1433;Initial Catalog=LuckyDB;User ID=****;Password=****;");      
    }
  }