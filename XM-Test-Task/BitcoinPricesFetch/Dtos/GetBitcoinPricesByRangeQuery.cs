namespace XM_Test_Task.BitcoinPricesFetch.Dtos;

public class GetBitcoinPricesByRangeQuery
{
    public long StartTicks { get; set; }
    public long EndTicks { get; set; }
}