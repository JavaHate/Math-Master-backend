namespace JavaHateBE.model
{
    public class Game : IEquatable<Game> {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public GameMode GameMode { get; set; }  
        public uint Score { get; set; } = 0;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public List<Question> Questions { get; private set; } = new List<Question>();
        public Guid UserId { get; set; }
        
        public Game(GameMode gameMode, Guid userId) {
            GameMode = gameMode;
            UserId = userId;
        }

        public Game() { }

        public void AddQuestion(Question question) {
            Questions.Add(question);
        }

        public void IncreaseScore() {
            Score++;
        }

        public void updateEndTime() {
            endTime = DateTime.Now;
        }

        public void updateEndTime(DateTime time) {
            endTime = time;
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
            startTime = time;
        }

        public void updateStartTime() {
            startTime = DateTime.Now;
        }

        public void updateUserId(Guid userId) {
            UserId = userId;
        }

        public bool Equals(Game? other) {
            if (other is null) return false;
            return Id == other.Id;
        }
    }
}