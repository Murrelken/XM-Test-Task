using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XM_Test_Task.BitcoinPricesFetch.Entities;
using XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;

namespace XM_Test_Task.BitcoinPricesFetch;

[ApiController]
[Route("[controller]/[action]")]
public class BitcoinPricesFetchController : ControllerBase
{
    private readonly IEnumerable<IExternalPriceSource> _externalPriceSources;
    private readonly DbContext _dbContext;

    public BitcoinPricesFetchController(IEnumerable<IExternalPriceSource> externalPriceSources, DbContext dbContext)
    {
        _externalPriceSources = externalPriceSources;
        _dbContext = dbContext;
    }

    [HttpGet("{ticks}")]
    public async Task<IActionResult> GetBySpecificTime(long ticks)
    {
        var ticksTrimmedToHours = ticks / 3600;

        var existingPriceInDatabase = await _dbContext
            .Set<BitcoinPriceByHour>()
            .FirstOrDefaultAsync(x => x.Hours == ticksTrimmedToHours);

        if (existingPriceInDatabase is null)
        {
            var prices = new List<decimal>();
            using HttpClient client = new();
            foreach (var formattedPriceSource in _externalPriceSources)
            {
                var uri = formattedPriceSource.GetRequestUri(ticks, ticks, 1);
                var response = await client.GetStringAsync(uri);
                var result = formattedPriceSource.GetPricesFromRawResponse(response);
                prices.Add(result[0]);
            }

            var aggregated = prices.Sum() / prices.Count;

            existingPriceInDatabase = new BitcoinPriceByHour(ticksTrimmedToHours, aggregated);

            _dbContext.Add(existingPriceInDatabase);
            await _dbContext.SaveChangesAsync();
        }

        return Ok(existingPriceInDatabase);
    }
}