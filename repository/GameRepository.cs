using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.model;

namespace JavaHateBE.repository
{
    public class GameRepository
    {
        public List<Game> gameList { get; private set; } = new List<Game>();

        public GameRepository() { }

        public async Task<Game> AddGame(Game game) {
            if (!gameList.Contains(game))
            {
                gameList.Add(game);
            }
            return await Task.FromResult(game);
        }

        public async Task<Game?> RemoveGame(Guid id) {
            Game? game = await GetGameById(id);
            if (game != null) {
                gameList.Remove(game);
            }
            return await Task.FromResult(game);
        }

        public async Task<Game?> GetGameById(Guid id) {
            return await Task.FromResult(gameList.FirstOrDefault(game => game.Id == id));
        }

        public async Task<List<Game>> GetAllGames() {
            return await Task.FromResult(gameList);
        }

        public async Task<List<Game>> GetGamesByUser(Guid userId) {
            return await Task.FromResult(gameList.Where(game => game.Gamer.Id == userId).ToList());
        }

        public async Task<List<Game>> GetGamesByUserByGameMode(Guid userId, GameMode gameMode) {
            return await Task.FromResult(gameList.Where(game => game.Gamer.Id == userId && game.GameMode == gameMode).ToList());
        }

        public async Task<List<Game>> GetGamesByGameMode(GameMode gameMode) {
            return await Task.FromResult(gameList.Where(game => game.GameMode == gameMode).ToList());
        }

        public async Task<Game?> UpdateGame(Game game) {
            Game? gameToUpdate = await GetGameById(game.Id);
            if (gameToUpdate != null) {
                gameToUpdate.updateEndTime(game.endTime);
                gameToUpdate.updateGameMode(game.GameMode);
                gameToUpdate.updateGamer(game.Gamer);
                gameToUpdate.updateQuestions(game.Questions);
                gameToUpdate.updateScore(game.Score);
                gameToUpdate.updateStartTime(game.startTime);
            }
            return await Task.FromResult(gameToUpdate);
        }
        
    }
}