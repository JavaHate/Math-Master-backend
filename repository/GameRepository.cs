using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavaHateBE.model;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.repository
{
    public class GameRepository
    {
        private readonly SampleDBContext _context;

        public GameRepository(SampleDBContext context)
        {
            _context = context;
        }

        public async Task<Game> AddGame(Game game) {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> RemoveGame(Guid id) {
            var game = await GetGameById(id);
            if (game != null) {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return game;
        }

        public async Task<Game?> GetGameById(Guid id) {
            return await _context.Games.FindAsync(id);
        }

        public async Task<List<Game>> GetAllGames() {
            return await _context.Games.ToListAsync();
        }

        public async Task<List<Game>> GetGamesByUser(Guid userId) {
            return await _context.Games.Where(game => game.Gamer != null && game.Gamer.Id == userId).ToListAsync();
        }

        public async Task<List<Game>> GetGamesByUserByGameMode(Guid userId, GameMode gameMode) {
            return await _context.Games.Where(game => game.Gamer != null && game.Gamer.Id == userId && game.GameMode == gameMode).ToListAsync();
        }

        public async Task<List<Game>> GetGamesByGameMode(GameMode gameMode) {
            return await _context.Games.Where(game => game.GameMode == gameMode).ToListAsync();
        }

        public async Task<Game?> UpdateGame(Game game) {
            Game? existingGame = await GetGameById(game.Id);
            if (existingGame == null)
            {
                return null;
            }

            existingGame.updateEndTime(game.endTime);
            existingGame.updateGameMode(game.GameMode);
            existingGame.updateQuestions(game.Questions);
            existingGame.updateScore(game.Score);
            existingGame.updateStartTime(game.startTime);
            if (game.Gamer != null) {
                existingGame.updateGamer(game.Gamer);
            }
            
            _context.Games.Update(existingGame);
            await _context.SaveChangesAsync();
            
            return existingGame;
        }
        
    }
}