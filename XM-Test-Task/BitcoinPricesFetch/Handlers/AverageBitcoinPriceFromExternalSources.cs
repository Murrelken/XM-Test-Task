namespace XM_Test_Task.BitcoinPricesFetch.Handlers;

public class AverageBitcoinPriceFromExternalSources : IAggregateBitcoinPricesFromExternalSources
{
    public decimal Aggregate(IEnumerable<decimal> prices)
    {
        var arr = prices as decimal[] ?? prices.ToArray();
        var res = arr.Sum() / arr.Length;
        return res;
    }
}