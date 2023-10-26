using CityInfo.Api.Controllers;
using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts;

public class CityInfoContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("New York City")
            { 
                Id = 1, 
                Description = "The one with that big park." 
            },
            new City("Antwerp")
            {
                Id = 2,
                Description = "That weird german band or IDK."
            },
            new City("Paris")
            {
                Id = 3,
                Description = "The one with that big tower."
            });

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Central Park")
            {
                Id = 1,
                CityId = 1,
                Description = "Big Park"
            },
            new PointOfInterest("Unfinished cathedral")
            {
                Id = 2,
                CityId = 2,
                Description = "Unfinished Cathedral"
            },
            new PointOfInterest("Eiffel Tower")
            {
                Id = 3,
                CityId = 3,
                Description = "Big tower"
            });
        base.OnModelCreating(modelBuilder);
    }
    // This is on way of configuring the database to the context. But we went with a different approach that can be seen in Program.cs

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite("connectionstring");
    //    base.OnConfiguring(optionsBuilder);
    //}
}
