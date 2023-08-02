using Microsoft.EntityFrameworkCore;
using XM_Test_Task.BitcoinPricesFetch.Dtos;
using XM_Test_Task.BitcoinPricesFetch.Entities;

namespace XM_Test_Task.BitcoinPricesFetch.Handlers;

public class GetBitcoinPricesByRangeFromDatabaseHandler
{
    private readonly DbContext _dbContext;

    public GetBitcoinPricesByRangeFromDatabaseHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BitcoinPricesDto>> Get(GetBitcoinPricesByRangeQuery query, CancellationToken ct)
    {
        var result = await _dbContext
            .Set<BitcoinPriceByHour>()
            .Where(BitcoinPriceByHour.ByTimeRangeSpec(query.StartTicks, query.EndTicks))
            .OrderBy(x => x.Hours)
            .Select(x => new BitcoinPricesDto
            {
                Time = x.Hours * 3600,
                Price = x.Price
            })
            .ToArrayAsync(ct);

        return result;
    }
}