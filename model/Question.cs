namespace JavaHateBE.model
{
    public class Question : IEquatable<Question>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; private set; }
        public float Answer { get; private set; }
        public byte Difficulty { get; private set; }

        public Question(string text, float answer, byte difficulty = 1)
        {
            Text = text;
            Answer = answer;
            Difficulty = difficulty;
        }

        public void UpdateText(string text)
        {
            Text = text;
        }

        public void UpdateAnswer(float answer)
        {
            Answer = answer;
        }

        public void UpdateDifficulty(byte difficulty)
        {
            Difficulty = difficulty;
        }

        public bool Equals(Question? other)
        {
            if (other is null) return false;
            return Id == other.Id;
        }
    }
}