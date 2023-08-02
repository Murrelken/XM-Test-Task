namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

public interface IExternalPriceSource
{
    string GetRequestUri(long startTicks);
    decimal GetPriceFromRawResponse(string response);
}