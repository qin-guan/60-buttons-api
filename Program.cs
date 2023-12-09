using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GameService>();

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

app.UseCors();

app.MapHub<PositionHub>("/Position");

app.MapGet("/Players", (GameService gameService) => gameService.Scores);
app.MapGet("/Live", (GameService gameService) => gameService.Position);

app.Run();