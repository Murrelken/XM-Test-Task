namespace XM_Test_Task.BitcoinPricesFetch.Handlers;

public interface IAggregateBitcoinPricesFromExternalSources
{
    decimal Aggregate(IEnumerable<decimal> prices);
}