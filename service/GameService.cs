using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.exceptions;
using JavaHateBE.model;
using JavaHateBE.repository;

namespace JavaHateBE.service
{
    public class GameService
    {
        private readonly GameRepository _GameRepository;
        private readonly UserRepository _userRepository;

        public GameService(GameRepository GameRepository, UserRepository userRepository)
        {
            _GameRepository = GameRepository;
            _userRepository = userRepository;
        }

        public async Task<Game> GetGameById(Guid id)
        {
            Game? Game = await _GameRepository.GetGameById(id) ?? throw new ObjectNotFoundException("Game", "No Games found with that ID");
            return await Task.FromResult(Game);
        }

        public async Task<Game> AddGame(Game game)
        {
            await _userRepository.GetUserById(game.Gamer.Id);
            return await _GameRepository.AddGame(game);
        }

        public async Task<Game?> RemoveGame(Guid id)
        {
            Game? game = await _GameRepository.GetGameById(id);
            if (game == null) {
                throw new ObjectNotFoundException("Game","No Games found with that ID");
            }
            return await _GameRepository.RemoveGame(id);
        }

        public async Task<List<Game>> GetAllGames()
        {
            return await _GameRepository.GetAllGames();
        }

        public async Task<List<Game>> GetGamesByUser(Guid userId)
        {
            await _userRepository.GetUserById(userId);
            return await _GameRepository.GetGamesByUser(userId);
        }

        public async Task<List<Game>> GetGamesByUserByGameMode(Guid userId, string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode);
            User? user = await _userRepository.GetUserById(userId);
            if(user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }
            return await _GameRepository.GetGamesByUserByGameMode(userId, mode);
        }

        public async Task<List<Game>> GetGamesByGameMode(string gameMode)
        {
            GameMode mode = Enum.Parse<GameMode>(gameMode);
            return await _GameRepository.GetGamesByGameMode(mode);
        }

        public async Task<Game> UpdateGame(Game game)
        {
            User? user = await _userRepository.GetUserById(game.Gamer.Id);
            if(user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }
            Game? newGame = await _GameRepository.UpdateGame(game);
            if(newGame == null)
            {
                throw new ObjectNotFoundException("Game", "No game found with that ID");
            }
            return newGame;
        }

        public async Task<Game> NewGame(Guid userId, string gameMode)
        {
            User? user = await _userRepository.GetUserById(userId);
            if(user == null)
            {
                throw new ObjectNotFoundException("User", "No user found with that ID");
            }
            GameMode mode = Enum.Parse<GameMode>(gameMode, true);
            Game game = new Game(mode, user);
            return await _GameRepository.AddGame(game);
        }
    }
}