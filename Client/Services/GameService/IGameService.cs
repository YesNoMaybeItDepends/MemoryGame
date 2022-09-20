using MemoryGame.Shared;

namespace MemoryGame.Client.Services.GameService
{
  public interface IGameService
  {
    List<Game> Games { get; set; }
    Task GetGames();
    Task CreateGame(Game game);
  }
}
