using Microsoft.EntityFrameworkCore;
using XM_Test_Task.BitcoinPricesFetch.Entities;

namespace XM_Test_Task.Database;

public class PriceFetchDbContext : DbContext
{
    public PriceFetchDbContext(DbContextOptions<PriceFetchDbContext> options) : base(options)
    {
    }

    public DbSet<BitcoinPriceByHour> BitcoinPricesByHour { get; set; }
}