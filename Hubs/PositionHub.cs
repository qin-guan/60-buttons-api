using Microsoft.AspNetCore.SignalR;

public class PositionHub(GameService _gameService) : Hub
{
    private readonly Random _random = new();
    
    public async Task Click()
    {
        _gameService.AddPoint(Context.ConnectionId);
        await Clients.All.SendAsync("NewPosition", new Position(_random.NextDouble(), _random.NextDouble()));
    }
}
