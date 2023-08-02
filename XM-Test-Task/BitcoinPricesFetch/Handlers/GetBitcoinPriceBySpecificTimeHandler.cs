using Microsoft.EntityFrameworkCore;
using XM_Test_Task.BitcoinPricesFetch.Entities;
using XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

namespace XM_Test_Task.BitcoinPricesFetch.Handlers;

public class GetBitcoinPriceBySpecificTimeHandler
{
    private readonly IEnumerable<IExternalPriceSource> _externalPriceSources;
    private readonly DbContext _dbContext;
    private readonly IAggregateBitcoinPricesFromExternalSources _aggregateBitcoinPricesFromExternalSources;

    public GetBitcoinPriceBySpecificTimeHandler(IEnumerable<IExternalPriceSource> externalPriceSources,
        DbContext dbContext, IAggregateBitcoinPricesFromExternalSources aggregateBitcoinPricesFromExternalSources)
    {
        _externalPriceSources = externalPriceSources;
        _dbContext = dbContext;
        _aggregateBitcoinPricesFromExternalSources = aggregateBitcoinPricesFromExternalSources;
    }

    public async Task<decimal> Get(long ticks, CancellationToken ct)
    {
        var ticksTrimmedToHours = ticks / 3600;

        var existingPriceInDatabase = await _dbContext
            .Set<BitcoinPriceByHour>()
            .FirstOrDefaultAsync(x => x.Hours == ticksTrimmedToHours, ct);

        if (existingPriceInDatabase is not null) return existingPriceInDatabase.Price;
        var aggregated = await GetPriceFromExternalSourcesAndAddItToDatabase(ticks, ticksTrimmedToHours, ct);

        return aggregated;
    }

    private async Task<decimal> GetPriceFromExternalSourcesAndAddItToDatabase(long ticks, long ticksTrimmedToHours,
        CancellationToken ct)
    {
        using HttpClient client = new ();
        client.Timeout = TimeSpan.FromSeconds(1);

        var externalPriceSourcesWithResults = _externalPriceSources
            .Select(externalPriceSource =>
            {
                var uri = externalPriceSource.GetRequestUri(ticks);
                var task = client.GetStringAsync(uri, ct);
                return new
                {
                    externalPriceSource,
                    task
                };
            })
            .ToArray();

        await Task.WhenAll(externalPriceSourcesWithResults.Select(x => x.task));

        var prices = new List<decimal>();
        foreach (var e in externalPriceSourcesWithResults)
        {
            var result = e.externalPriceSource.GetPriceFromRawResponse(e.task.Result);
            prices.Add(result);
        }

        var aggregated = _aggregateBitcoinPricesFromExternalSources.Aggregate(prices);

        var newPrice = new BitcoinPriceByHour(ticksTrimmedToHours, aggregated);

        _dbContext.Add(newPrice);
        await _dbContext.SaveChangesAsync(ct);
        return aggregated;
    }
}