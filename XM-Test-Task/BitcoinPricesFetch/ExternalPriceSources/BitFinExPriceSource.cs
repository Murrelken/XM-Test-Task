using System.Text.Json;

namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

public class BitFinExPriceSource : IExternalPriceSource
{
    public int MaxLimit => 10000;

    public string GetRequestUri(long startTicks, long endTicks, int limit) =>
        $"https://api-pub.bitfinex.com/v2/candles/trade:1h:tBTCUSD/hist?limit={limit}&sort=1&start={startTicks}000&end={endTicks}001";

    public IList<decimal> GetPricesFromRawResponse(string response)
    {
        var deserialized = JsonSerializer.Deserialize<List<List<decimal>>>(response);

        if (deserialized is null)
            throw new ArgumentException("Input response was deserialized into null.", nameof(response));

        return deserialized.Select(x => x[2]).ToArray();
    }
}