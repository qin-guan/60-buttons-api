using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class GameService(AppDbContext dbContext, IMemoryCache cache)
{
    private readonly Random _random = new();

    public Position Position =>
        cache.GetOrCreate("Position", cacheEntry => new Position(_random.NextDouble(), _random.NextDouble())) ??
        throw new InvalidOperationException();

    public async Task<IEnumerable<Player>> Leaderboard()
    {
        return await dbContext.Players.ToListAsync();
    }

    public async Task Click(Guid id)
    {
        cache.Set("Position", new Position(_random.NextDouble(), _random.NextDouble()));

        var player = await dbContext.Players.FindAsync(id) ?? throw new InvalidOperationException();
        player.Score++;

        await dbContext.SaveChangesAsync();
    }
}