using JavaHateBE.Model.DTOs;
using Newtonsoft.Json;

namespace JavaHateBE.Model
{
    public class Game : IEquatable<Game> {
        public Guid Id { get; private set; } 
        public GameMode GameMode { get; set; }  
        public uint Score { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Question> Questions { get; private set; } = new List<Question>();
        public Guid UserId { get; set; }
        
        public Game(GameMode gameMode, Guid userId) {
            GameMode = gameMode;
            UserId = userId;
            updateStartTime();
            Id = Guid.NewGuid();
        }

        [JsonConstructor]
        public Game(Guid id, GameMode gameMode, uint score, DateTime startTime, DateTime endTime, List<Question> questions, Guid userId) {
            Id = id;
            GameMode = gameMode;
            Score = score;
            StartTime = startTime;
            EndTime = endTime;
            Questions = questions;
            UserId = userId;
        }

        public Game() {}

        public void AddQuestion(Question question) {
            Questions.Add(question);
        }

        public void IncreaseScore() {
            Score++;
        }

        public void updateEndTime() {
            EndTime = DateTime.Now;
        }

        public void updateEndTime(DateTime time) {
            EndTime = time;
        }

        public void updateScore(uint score) {
            Score = score;
        }

        public void updateQuestions(List<Question> questions) {
            Questions = questions;
        }

        public void updateGameMode(GameMode gameMode) {
            GameMode = gameMode;
        }

        public void updateStartTime(DateTime time) {
            StartTime = time;
        }

        public void updateStartTime() {
            StartTime = DateTime.Now;
        }

        public void updateUserId(Guid userId) {
            UserId = userId;
        }

        public bool Equals(Game? other) {
            if (other is null) return false;
            return Id == other.Id;
        }

        public static Game From(UpdateGameInput input) {
            return new Game(input.Id, input.GameMode, input.Score, input.StartTime, input.EndTime, input.Questions, input.UserId);
        }
    }
}