public class GameService(IDictionary<string, int> scores)
{
    public void AddPoint(string id)
    {
        if (scores.ContainsKey(id))
        {
            scores.Add(id, 1);
        }
        else
        {
            scores[id]++;
        }
    }
}