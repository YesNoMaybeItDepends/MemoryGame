namespace MemoryGame.Shared
{
  public class Game
  {
    public int Attempts { get; set; }
    public TimeSpan Time { get; set; }

    public Game(int attempts, TimeSpan time)
    {
      Attempts = attempts;
      Time = time;
    }
  }
}
