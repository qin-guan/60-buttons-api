using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var honeycombOptions = builder.Configuration.GetHoneycombOptions();

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeScopes = true;
    logging.IncludeFormattedMessage = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddConsoleExporter()
            .AddHoneycomb(honeycombOptions)
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation();
    })
    .WithTracing(tracing =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // We want to view all traces in development
            tracing.SetSampler(new AlwaysOnSampler());
        }

        tracing
            .AddHoneycomb(honeycombOptions)
            .AddConsoleExporter()
            .AddAspNetCoreInstrumentation()
            .AddCommonInstrumentations()
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
            });
    });

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(honeycombOptions.ServiceName));
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<PlayerService>();

builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("App"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowCredentials();
        policy.WithOrigins(
            builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
            ?? throw new InvalidOperationException()
        );
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

using var scope = app.Services.CreateScope();
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseCors();

app.MapHub<PositionHub>("/Position");

var game = app.MapGroup("/Game");
game.MapGet("/Position", (GameService gameService) => gameService.Position);
game.MapGet("/Leaderboard", async (GameService gameService) => await gameService.Leaderboard());

var player = app.MapGroup("/Player");
player.MapPost("/", async (PlayerService playerService, HttpRequest request) =>
{
    var body = await request.ReadFromJsonAsync<RegisterPlayerDto>();
    if (body == null) throw new BadHttpRequestException("Invalid body");
    return await playerService.Register(body.Name);
});
player.MapGet("/{id:required}", async (PlayerService playerService, string id) =>
{
    if (!Guid.TryParse(id, out var guid))
    {
        throw new BadHttpRequestException("Invalid User ID provided");
    }

    return await playerService.Find(guid);
});

app.Run();

public record RegisterPlayerDto(
    [Required] [MinLength(1)] string Name
);