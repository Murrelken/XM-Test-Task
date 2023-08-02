using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace XM_Test_Task.BitcoinPricesFetch.Entities;

[PrimaryKey(nameof(Hours))]
public class BitcoinPriceByHour
{
    public static Expression<Func<BitcoinPriceByHour, bool>> ByTimeRangeSpec(long startTicks, long endTicks) =>
        x => x.Hours >= startTicks / 3600 && x.Hours <= endTicks / 3600;

    public BitcoinPriceByHour(long hours, decimal price)
    {
        Hours = hours;
        Price = price;
    }

    public long Hours { get; set; }

    public decimal Price { get; set; }
}