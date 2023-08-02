using System.Reflection;
using Microsoft.EntityFrameworkCore;
using XM_Test_Task.BitcoinPricesFetch.ExternalPriceSources;
using XM_Test_Task.BitcoinPricesFetch.Handlers;
using XM_Test_Task.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PriceFetchDbContext>
    (o => o.UseInMemoryDatabase("PriceFetchInMemory"));

builder.Services.AddScoped<DbContext, PriceFetchDbContext>();

Assembly.GetEntryAssembly()
    ?.GetTypes()
    .Where(t => typeof(IExternalPriceSource).IsAssignableFrom(t))
    .Where(t => t is { IsInterface: false, IsAbstract: false })
    .ToList()
    .ForEach(t =>
    {
        builder.Services.AddScoped(typeof(IExternalPriceSource), t);
    });

builder.Services.AddScoped<GetBitcoinPriceBySpecificTimeHandler>();
builder.Services.AddScoped<GetBitcoinPricesByRangeFromDatabaseHandler>();
builder.Services.AddScoped<IAggregateBitcoinPricesFromExternalSources, AverageBitcoinPriceFromExternalSources>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();