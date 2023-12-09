using Microsoft.AspNetCore.SignalR;

public class PositionHub(GameService gameService) : Hub
{
    public async Task Click()
    {
        gameService.Click(Context.ConnectionId);
        await Clients.All.SendAsync("Update", new { gameService.Position, gameService.Scores });
    }
}