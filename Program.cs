using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<GameService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowCredentials();
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors();
app.MapHub<PositionHub>("/Position");

app.Run();
