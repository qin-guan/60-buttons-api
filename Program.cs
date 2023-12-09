var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<GameService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowCredentials();
        policy.WithOrigins(
            builder.Configuration.GetValue<string[]>("Cors.Origins", new[] { "http://localhost:3000" }) ??
            throw new InvalidOperationException());
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