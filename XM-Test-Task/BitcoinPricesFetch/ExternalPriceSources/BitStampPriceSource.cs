using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

[UsedImplicitly]
public class BitStampPriceSource : IExternalPriceSource
{
    public string GetRequestUri(long startTicks) =>
        $"https://www.bitstamp.net/api/v2/ohlc/btcusd/?step=3600&limit=1&start={startTicks}";

    public decimal GetPriceFromRawResponse(string response)
    {
        var pricesArray = (JArray?)JObject.Parse(response)["data"]?["ohlc"];

        var res = pricesArray?
                      .Select(x => (decimal)(x["close"] ?? 0))
                      .FirstOrDefault()
                  ?? throw new ArgumentException("Price isn't found in the response.", nameof(response));

        return res;
    }
}