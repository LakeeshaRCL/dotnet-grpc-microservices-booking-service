using BookingService.Helpers;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService;

public class BookingDbContext : DbContext
{
    private readonly MySqlConfiguration mySqlConfiguration;

    public BookingDbContext(GlobalSingletonProperties globalSingletonProperties)
    {
        this.mySqlConfiguration = globalSingletonProperties.mySqlConfiguration;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(mySqlConfiguration.mySqlConnection, mySqlConfiguration.GetMySqlVersion());
        }
    }
    
    
    public DbSet<BookingModel> Bookings { get; set; }
}