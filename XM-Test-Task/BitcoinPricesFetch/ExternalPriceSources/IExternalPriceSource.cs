namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

public interface IExternalPriceSource
{
    int MaxLimit { get; }
    string GetRequestUri(long startTicks, long endTicks, int limit);
    IList<decimal> GetPricesFromRawResponse(string response);
}