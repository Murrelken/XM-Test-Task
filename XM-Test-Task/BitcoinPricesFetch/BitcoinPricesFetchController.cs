using Microsoft.AspNetCore.Mvc;
using XM_Test_Task.BitcoinPricesFetch.Dtos;
using XM_Test_Task.BitcoinPricesFetch.Handlers;

namespace XM_Test_Task.BitcoinPricesFetch;

[ApiController]
[Route("[controller]/[action]")]
public class BitcoinPricesFetchController : ControllerBase
{
    /// <summary>
    /// Getting aggregated btc-usd price by specific time point
    /// </summary>
    /// <param name="ticks">The Unix epoch format</param>
    [HttpGet("{ticks}")]
    public async Task<ActionResult<decimal>> GetBySpecificTime(
        [FromServices] GetBitcoinPriceBySpecificTimeHandler getBitcoinPriceBySpecificTimeHandler,
        long ticks, CancellationToken ct)
    {
        var price = await getBitcoinPriceBySpecificTimeHandler.Get(ticks, ct);
        return Ok(price);
    }

    /// <summary>
    /// Getting a list of btc-usd prices from data storage by given time range
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BitcoinPricesDto>>> GetByRange(
        [FromServices] GetBitcoinPricesByRangeFromDatabaseHandler getBitcoinPricesByRangeFromDatabaseHandler,
        [FromQuery] GetBitcoinPricesByRangeQuery query, CancellationToken ct)
    {
        var prices = await getBitcoinPricesByRangeFromDatabaseHandler.Get(query, ct);
        return Ok(prices);
    }
}