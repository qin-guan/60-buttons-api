public class GameService
{
    private readonly Random _random = new();
    public Position Position { get; private set; }
    public IDictionary<string, int> Scores { get; } = new Dictionary<string, int>();

    public GameService()
    {
        Position = new Position(_random.NextDouble(), _random.NextDouble());
    }

    public void Click(string connectionId)
    {
        if (!Scores.ContainsKey(connectionId))
        {
            Scores.Add(connectionId, 1);
        }
        else
        {
            Scores[connectionId]++;
        }
        
        Position = new Position(_random.NextDouble(), _random.NextDouble());
    }
}