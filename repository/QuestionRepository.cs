using JavaHateBE.model;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.repository{
    
    public class QuestionRepository {
        private readonly MathMasterDBContext _context;

        public QuestionRepository(MathMasterDBContext context)
        {
            _context = context;
        }

        public async Task<Question?> GetQuestionById(Guid id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> AddQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question?> UpdateQuestion(Question question)
        {
            var existingQuestion = await GetQuestionById(question.Id);
            if (existingQuestion == null)
            {
                return null;
            }
            existingQuestion.UpdateText(question.Text);
            existingQuestion.UpdateAnswer(question.Answer);
            await _context.SaveChangesAsync();
            return existingQuestion;
        }

        public async Task<Question?> DeleteQuestion(Guid id)
        {
            var question = await GetQuestionById(id);
            if (question == null)
            {
                return null;
            }
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question?> GetQuestionFromText(string text)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Text == text);
        }
    }
}