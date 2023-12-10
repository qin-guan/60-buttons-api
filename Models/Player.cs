public class Player(string name, int score)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
    public int Score { get; set; } = score;
}