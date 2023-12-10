public class PlayerService(AppDbContext dbContext)
{
    public async Task<Player> Find(Guid id)
    {
        return await dbContext.Players.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<Player> Register(string name = "Nameless player")
    {
        var player = new Player(name, 0);
        await dbContext.Players.AddAsync(player);
        await dbContext.SaveChangesAsync();
        return player;
    }
}