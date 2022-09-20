using MemoryGame.Shared;
using System.Net.Http.Json;

namespace MemoryGame.Client.Services.GameService
{
  public class GameService : IGameService
  {
    private readonly HttpClient _http;
    private readonly ILogger<GameService> _logger;
    public GameService(HttpClient http, ILogger<GameService> logger)
    {
      _http = http;
      _logger = logger;
    }

    public List<Game> Games { get; set; } = new List<Game>();

    public async Task CreateGame(Game game)
    {
      var result = await _http.PutAsJsonAsync($"api/game", game);
    }

    public async Task GetGames()
    {
      _logger.LogInformation("getting games");
      if (Games.Count == 0)
      {
        var result = await _http.GetFromJsonAsync<List<Game>>("api/game");
        if (result != null)
        {
          Games = result;
        }
      }
    }
  }
}
