using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//connection string located in appsettings.json
builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthorization();
// app.MapControllers();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// All of these .with() clauses at the end are moreorless for documentation
app.MapGet("/api/SuperHero", async (ILogger<Program> _logger, DataContext _dbContext) =>
{
    _logger.Log(logLevel: LogLevel.Information, message: "****** Getting all super heroes ******");
    var superHeroes = await _dbContext.SuperHeroes.ToListAsync();

    return Results.Ok(superHeroes);
})
.WithName("GetAllSuperHeroes")
.WithOpenApi();

app.MapGet("api/SuperHero/{id:int}", async (int id, DataContext _dbContext) => 
{
    var hero = await _dbContext.SuperHeroes.FindAsync(id);
    if (hero is null)
        return Results.NotFound();

    return Results.Ok(hero);
})
.WithName("Get single hero")
.WithOpenApi();

app.MapPost("api/SuperHero", async (SuperHero hero, DataContext _dbContext) => {
    _dbContext.SuperHeroes.Add(hero);
    await _dbContext.SaveChangesAsync();

    return Results.Ok(hero);
});

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
