using JavaHateBE.model;
using JavaHateBE.exceptions;

namespace JavaHateBE.repository{
    
    public class QuestionRepository {
        public List<Question> Questions { get; private set; } = new List<Question>();
        public QuestionRepository()
        {
            for(int i = 1; i <= 15; i++)
            {
                for(int j = 0; j < 15; j++)
                {
                    Questions.Add(new Question($"{i} + {j}", i + j, 1));
                }
            }
        }

        public async Task<Question?> GetQuestionById(Guid id)
        {
            return await Task.FromResult(Questions.Find(q => q.Id == id));
        }

        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await Task.FromResult(Questions);
        }

        public async Task<Question> AddQuestion(Question question)
        {
            Questions.Add(question);
            return await Task.FromResult(question);
        }

        public async Task<Question?> UpdateQuestion(Question question)
        {
            Question? currentQuestion = await GetQuestionById(question.Id);
            if (currentQuestion == null)
            {
                return await Task.FromResult(currentQuestion);
            }
            currentQuestion.UpdateText(question.Text);
            currentQuestion.UpdateAnswer(question.Answer);
            return await Task.FromResult(question);
        }

        public async Task<Question?> DeleteQuestion(Guid id)
        {
            Question? question = await GetQuestionById(id);
            if (question == null)
            {
                return await Task.FromResult(question);
            }
            Questions.Remove(question);
            return await Task.FromResult(question);
        }

        public async Task<Question?> GetQuestionFromText(string text)
        {
            return await Task.FromResult(Questions.Find(q => q.Text == text));
        }
    }
}