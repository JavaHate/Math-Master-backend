using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.exceptions;
using JavaHateBE.model;
using JavaHateBE.service;
using Microsoft.AspNetCore.Mvc;

namespace JavaHateBE.controller
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        private readonly GameService _gameService;

        public GameController(ILogger<GameController> logger, GameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Game>> GetGameById([FromRoute] Guid id)
        {
            try
            {
                Game game = await _gameService.GetGameById(id);
                return Ok(game);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Game>> AddGame([FromBody] Game game)
        {
            try
            {
                Game newGame = await _gameService.AddGame(game);
                return Ok(newGame);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult<Game>> RemoveGame([FromRoute] Guid id)
        {
            try
            {
                Game? game = await _gameService.RemoveGame(id);
                if (game == null)
                {
                    return NotFound();
                }
                return Ok(game);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAllGames()
        {
            try
            {
                List<Game> games = await _gameService.GetAllGames();
                return Ok(games);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Game>>> GetGamesByUser([FromRoute] Guid userId)
        {
            try
            {
                List<Game> games = await _gameService.GetGamesByUser(userId);
                return Ok(games);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("user/{userId}/gameMode/{gameMode}")]
        public async Task<ActionResult<List<Game>>> GetGamesByUserByGameMode([FromRoute] Guid userId, [FromRoute] string gameMode)
        {
            try
            {
                List<Game> games = await _gameService.GetGamesByUserByGameMode(userId, gameMode);
                return Ok(games);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("gameMode/{gameMode}")]
        public async Task<ActionResult<List<Game>>> GetGamesByGameMode([FromRoute] string gameMode)
        {
            try
            {
                List<Game> games = await _gameService.GetGamesByGameMode(gameMode);
                return Ok(games);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Game>> UpdateGame([FromBody] Game game)
        {
            try
            {
                Game updatedGame = await _gameService.UpdateGame(game);
                return Ok(updatedGame);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPost("user/{userId}/gameMode/{gameMode}")]
        public async Task<ActionResult<Game>> NewGame([FromRoute] Guid userId, [FromRoute] string gameMode)
        {
            try
            {
                Game newGame = await _gameService.NewGame(userId, gameMode);
                return Ok(newGame);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("generate-problem")]
        public ActionResult<object> GenerateProblem()
        {
            try
            {
                Random rand = new Random();
                int num1 = rand.Next(1,11); //this also needs to be changed to a variable later on
                int num2 = rand.Next(1,11);
                string[] operators = {"+", "-", "*"};
                string selectedOperator = operators [rand.Next(operators.Length)];

                string problem = $"{num1} {selectedOperator} {num2}";

                return Ok(new { problem });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to generate a math problem.");
                return BadRequest(new { message = "Error generating problem" });
            }
        }
    }
}