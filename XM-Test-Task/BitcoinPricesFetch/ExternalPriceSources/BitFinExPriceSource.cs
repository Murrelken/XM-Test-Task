using System.Text.Json;
using JetBrains.Annotations;

namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

[UsedImplicitly]
public class BitFinExPriceSource : IExternalPriceSource
{
    public string GetRequestUri(long startTicks) =>
        $"https://api-pub.bitfinex.com/v2/candles/trade:1h:tBTCUSD/hist?limit=1&sort=1&start={startTicks}000";

    public decimal GetPriceFromRawResponse(string response)
    {
        var deserialized = JsonSerializer.Deserialize<List<List<decimal>>>(response);

        if (deserialized is null)
            throw new ArgumentException("Input response was deserialized into null.", nameof(response));

        var res = deserialized
                      .FirstOrDefault()?[2]
                  ?? throw new ArgumentException("Price isn't found in the response.", nameof(response));

        return res;
    }
}