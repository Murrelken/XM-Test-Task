namespace XM_Test_Task.BitcoinPricesFetch.Dtos;

public class GetBitcoinPricesByRangeQuery
{
    /// <summary>
    /// The Unix epoch format
    /// </summary>
    public long StartTicks { get; set; }
    /// <summary>
    /// The Unix epoch format
    /// </summary>
    public long EndTicks { get; set; }
}