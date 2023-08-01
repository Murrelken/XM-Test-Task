using Microsoft.EntityFrameworkCore;

namespace XM_Test_Task.BitcoinPricesFetch.Entities;

[PrimaryKey(nameof(Hours))]
public class BitcoinPriceByHour
{
    public BitcoinPriceByHour(long hours, decimal price)
    {
        Hours = hours;
        Price = price;
    }

    public long Hours { get; set; }

    public decimal Price { get; set; }
}