using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

public class BitStampPriceSource : IExternalPriceSource
{
    public int MaxLimit => 1000;

    public string GetRequestUri(long startTicks, long endTicks, int limit) =>
        $"https://www.bitstamp.net/api/v2/ohlc/btcusd/?step=3600&limit={limit}&start={startTicks}&end={endTicks}";

    public IList<decimal> GetPricesFromRawResponse(string response)
    {
        var pricesArray = (JArray?)JObject.Parse(response)["data"]?["ohlc"];

        var valuesArray = pricesArray?
            .Select(x => (decimal)(x["close"] ?? 0))
            .ToArray() ?? Array.Empty<decimal>();

        return valuesArray;
    }
}