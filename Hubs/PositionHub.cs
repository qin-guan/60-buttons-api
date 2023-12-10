using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

public class PositionHub(GameService gameService, IMemoryCache memoryCache) : Hub
{
    public void Hello(string userId)
    {
        memoryCache.Set(Context.ConnectionId, userId);
    }

    public async Task Click()
    {
        if (!memoryCache.TryGetValue<string>(Context.ConnectionId, out var userIdValue))
        {
            throw new Exception("User did not send hello");
        }

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new Exception("User is invalid");
        }
        
        await gameService.Click(userId);
        await Clients.All.SendAsync("Update",
            new
            {
                gameService.Position,
                Leaderboard = await gameService.Leaderboard()
            }
        );
    }
}