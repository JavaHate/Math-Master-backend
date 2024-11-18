using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.Exceptions;
using JavaHateBE.Model;
using JavaHateBE.Repository;
using JavaHateBE.Util;

namespace JavaHateBE.Service
{
    /// <summary>
    /// Provides services for managing games.
    /// </summary>
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly UserRepository _userRepository;
        private readonly ConcurrentDictionary<Guid, Game> _gamesCache = new ConcurrentDictionary<Guid, Game>();
        private readonly Validator<Game> _validator = new Validator<Game>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="gameRepository">The game repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public GameService(GameRepository gameRepository, UserRepository userRepository)
        {
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a game by its ID.
        /// </summary>
        public async Task<Game> GetGameById(Guid id)
        {
            if (_gamesCache.TryGetValue(id, out Game? cachedGame))
            {
                return await Task.FromResult(cachedGame);
            }

            Game? game = await _gameRepository.GetGameById(id) 
                         ?? throw new ObjectNotFoundException("Game", "No Games found with that ID");
            _gamesCache[id] = game;
            return game;
        }

        /// <summary>
        /// Adds a new game.
        /// </summary>
        public async Task<Game> AddGame(Game game)
        {
            await _userRepository.GetUserById(game.UserId);
            Game addedGame = await _GameRepository.AddGame(game);
            _gamesCache[addedGame.Id] = addedGame;
            return addedGame;
        }

        /// <summary>
        /// Removes a game by its ID.
        /// </summary>
        public async Task<Game?> RemoveGame(Guid id)
        {
            Game? game = await _gameRepository.GetGameById(id);
            if (game == null)
            {
                throw new ObjectNotFoundException("Game", "No Games found with that ID");
            }

            _gamesCache.TryRemove(id, out _);
            return await _gameRepository.RemoveGame(id);
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        public async Task<List<Game>> GetAllGames()
        {
            return await _GameRepository.GetAllGames();
        }

        /// <summary>
        /// Gets games by user ID.
        /// </summary>
        public async Task<List<Game>> GetGamesByUser(Guid userId)
        {
            await _userRepository.GetUserById(userId);
            return await _GameRepository.GetGamesByUser(userId);
        }

        /// <summary>
        /// Gets games by user ID and game mode.
        /// </summary>
        public async Task<List<Game>> GetGamesByUserByGameMode(Guid userId, string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode, true);
            User? user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }

            return await _gameRepository.GetGamesByUserByGameMode(userId, mode);
        }

        /// <summary>
        /// Gets games by game mode.
        /// </summary>
        public async Task<List<Game>> GetGamesByGameMode(string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode, true);
            return await _gameRepository.GetGamesByGameMode(mode);
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        public async Task<Game> UpdateGame(Game game)
        {
            if (!_validator.Validate(game, game.UserId))
            {
                throw new ArgumentException("Invalid game entity or user ID.");
            }

            User? user = await _userRepository.GetUserById(game.UserId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }

            Game? updatedGame = await _gameRepository.UpdateGame(game);
            if (updatedGame == null)
            {
                throw new ObjectNotFoundException("Game", "No game found with that ID");
            }

            _gamesCache[updatedGame.Id] = updatedGame;
            return updatedGame;
        }

        /// <summary>
        /// Creates a new game for a user with a specified game mode.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="gameMode">The game mode.</param>
        /// <returns>The newly created game.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified ID.</exception>
        public async Task<Game> NewGame(Guid userId, string gameMode)
        {
            User? user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }

            GameMode mode = Enum.Parse<GameMode>(gameMode, true);
            Game game = new Game(mode, user.Id);
            return await _GameRepository.AddGame(game);
        }
    }
}
