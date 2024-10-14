using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.exceptions;
using JavaHateBE.model;
using JavaHateBE.repository;

namespace JavaHateBE.service
{
    /// <summary>
    /// Provides services for managing games.
    /// </summary>
    public class GameService
    {
        private readonly GameRepository _GameRepository;
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="GameRepository">The game repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public GameService(GameRepository GameRepository, UserRepository userRepository)
        {
            _GameRepository = GameRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a game by its ID.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <returns>The game with the specified ID.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no game is found with the specified ID.</exception>
        public async Task<Game> GetGameById(Guid id)
        {
            Game? Game = await _GameRepository.GetGameById(id) ?? throw new ObjectNotFoundException("Game", "No Games found with that ID");
            return await Task.FromResult(Game);
        }

        /// <summary>
        /// Adds a new game.
        /// </summary>
        /// <param name="game">The game to add.</param>
        /// <returns>The added game.</returns>
        public async Task<Game> AddGame(Game game)
        {
            await _userRepository.GetUserById(game.UserId);
            return await _GameRepository.AddGame(game);
        }

        /// <summary>
        /// Removes a game by its ID.
        /// </summary>
        /// <param name="id">The ID of the game to remove.</param>
        /// <returns>The removed game, or null if no game was found with the specified ID.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no game is found with the specified ID.</exception>
        public async Task<Game?> RemoveGame(Guid id)
        {
            Game? game = await _GameRepository.GetGameById(id);
            if (game == null)
            {
                throw new ObjectNotFoundException("Game", "No Games found with that ID");
            }
            return await _GameRepository.RemoveGame(id);
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns>A list of all games.</returns>
        public async Task<List<Game>> GetAllGames()
        {
            return await _GameRepository.GetAllGames();
        }

        /// <summary>
        /// Gets games by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of games associated with the specified user.</returns>
        public async Task<List<Game>> GetGamesByUser(Guid userId)
        {
            await _userRepository.GetUserById(userId);
            return await _GameRepository.GetGamesByUser(userId);
        }

        /// <summary>
        /// Gets games by user ID and game mode.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="gameMode">The game mode.</param>
        /// <returns>A list of games associated with the specified user and game mode.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified ID.</exception>
        public async Task<List<Game>> GetGamesByUserByGameMode(Guid userId, string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode);
            User? user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }
            return await _GameRepository.GetGamesByUserByGameMode(userId, mode);
        }

        /// <summary>
        /// Gets games by game mode.
        /// </summary>
        /// <param name="gameMode">The game mode.</param>
        /// <returns>A list of games with the specified game mode.</returns>
        public async Task<List<Game>> GetGamesByGameMode(string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode);
            return await _GameRepository.GetGamesByGameMode(mode);
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="game">The game to update.</param>
        /// <returns>The updated game.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user or game is found with the specified ID.</exception>
        public async Task<Game> UpdateGame(Game game)
        {
            User? user = null;
            user = await _userRepository.GetUserById(game.UserId);
            if (user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }
            Game? newGame = await _GameRepository.UpdateGame(game);
            if (newGame == null)
            {
                throw new ObjectNotFoundException("Game", "No game found with that ID");
            }
            return newGame;
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