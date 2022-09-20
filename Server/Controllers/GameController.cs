using Microsoft.AspNetCore.Mvc;
using MemoryGame.Shared;
//using MemoryGame.Server.Data;
using Microsoft.Data.Sqlite;

namespace MemoryGame.Server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class GameController : ControllerBase
  {
    private readonly ILogger<GameController> _logger;
    private readonly IConfiguration _config;

    public GameController(ILogger<GameController> logger, IConfiguration config)
    {
      _logger = logger;
      _config = config;
      SetUpDb();
    }

    private void SetUpDb()
    {
      using (var db = new SqliteConnection(_config.GetConnectionString("Defaultconnection")))
      {
        db.Open();

        using (var command = db.CreateCommand())
        {
          _logger.LogInformation("yeeep");
          command.CommandText = Properties.Resources.CREATE_TABLE_Games;
          try
          {
            command.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            throw new Exception(ex.Message);
          }
          finally
          {
            command.Dispose();
            db.Dispose();
          }
        }
      }
    }

    private async Task InsertGame(Game game)
    {
      using (var connection = new SqliteConnection(_config.GetConnectionString("Defaultconnection")))
      {
        using (var command = connection.CreateCommand())
        {
          connection.Open();
          command.CommandText = Properties.Resources.INSERT_INTO_Games;
          command.Parameters.AddWithValue("@Attempts", game.Attempts);
          command.Parameters.AddWithValue("@Time", game.Time.TotalMilliseconds);
          await command.ExecuteNonQueryAsync();
        }
      }
    }

    private async Task<List<Game>> DbGetGames()
    {
      _logger.LogInformation("sneed");
      using (var connection = new SqliteConnection(_config.GetConnectionString("Defaultconnection")))
      {
        using (var command = connection.CreateCommand())
        {
          connection.Open();
          command.CommandText = "select * from Games";

          var games = new List<Game>();

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (reader.Read())
            {
              int attempts = reader.GetInt32(1);
              int time = reader.GetInt32(2);
              games.Add(new Game(attempts, TimeSpan.FromMilliseconds(time)));
            }
          }

          return games;
        }
      }
    }
    public List<Game> Games = new List<Game>()
    {
      new Game(1, new TimeSpan(0,0,30)),
      new Game(100, new TimeSpan(0,0,120)),
    };

    [HttpGet]
    public async Task<ActionResult<List<Game>>> GetGames()
    {
      _logger.LogInformation("controller get");
      return Ok(Games);
      var games = await DbGetGames();
      _logger.LogWarning(games.Count.ToString());
      if (games.Count != 0) return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGame(Game game)
    {
      Games.Add(game);
      return Ok();
    }
  }
}
