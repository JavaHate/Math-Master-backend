namespace JavaHateBE.model
{
    public class Game {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public GameMode GameMode { get; private set; }
        public uint Score { get; private set; } = 0;
        public DateTime startTime { get; private set; }
        public DateTime endTime { get; private set; }
        public List<Question> Questions { get; private set; } = new List<Question>();
        public User Gamer { get; private set; }
        
        public Game(GameMode gameMode, User user) {
            GameMode = gameMode;
            Gamer = user;
        }

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

        public void updateGamer(User user) {
            Gamer = user;
        }
    }
}