using Xunit; 
using JavaHateBE.Model;

namespace JavaHateBE.Test.Domain
{
    public class QuestionTest {
        private readonly String ValidText = "1 + 1";
        private readonly double ValidAnswer = 2;
        private readonly byte ValidDifficulty = 5;

        [Fact]
        public void CreatingValidQuestion_CreatesQuestion() {
            var Question = new Question(ValidText, ValidAnswer, ValidDifficulty);

            Assert.NotNull(Question);
            Assert.NotEqual(Guid.Empty, Question.Id);
            Assert.Equal(ValidText, Question.Text);
            Assert.Equal(ValidAnswer, Question.Answer);
            Assert.Equal(ValidDifficulty, Question.Difficulty);
        }

        [Fact]
        public void CreatingValidQuestionWithoutDifficulte_SetsDifficultyTo1AndCreatesQuestion() {
            var Question = new Question(ValidText, ValidAnswer);

            Assert.NotNull(Question);
            Assert.NotEqual(Guid.Empty, Question.Id);
            Assert.Equal(ValidText, Question.Text);
            Assert.Equal(ValidAnswer, Question.Answer);
            Assert.Equal(1, Question.Difficulty);
        }

        [Fact]
        public void UpdatingText_UpdatesText() {
            var Question = new Question(ValidText, ValidAnswer, ValidDifficulty);
            var newText = "2 + 2";

            Question.UpdateText(newText);

            Assert.Equal(newText, Question.Text);
        }

        [Fact]
        public void UpdatingAnswer_UpdatesAnswer() {
            var Question = new Question(ValidText, ValidAnswer, ValidDifficulty);
            var newAnswer = 4;

            Question.UpdateAnswer(newAnswer);

            Assert.Equal(newAnswer, Question.Answer);
        }

        [Fact]
        public void UpdatingDifficulty_UpdatesDifficulty() {
            var Question = new Question(ValidText, ValidAnswer, ValidDifficulty);
            byte newDifficulty = 10;

            Question.UpdateDifficulty(newDifficulty);

            Assert.Equal(newDifficulty, Question.Difficulty);
        }
        
        [Fact]
        public void Equals_ReturnsTrue_WhenComparingTwoQuestionsWithSameId() {
            var Question1 = new Question(ValidText, ValidAnswer, ValidDifficulty);
            var Question2 = new Question(ValidText, ValidAnswer, ValidDifficulty);

            Assert.True(Question1.Equals(Question2));
        }
    }
}