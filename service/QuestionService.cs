namespace JavaHateBE.service {
    using System;
    using System.Collections.Generic;
    using JavaHateBE.model;
    using JavaHateBE.repository;
    using JavaHateBE.exceptions;

    public class QuestionService {
        private readonly QuestionRepository _questionRepository;

        public QuestionService(QuestionRepository questionRepository) {
            _questionRepository = questionRepository;
        }

        public async Task<Question> GetQuestionById(Guid id) {
            Question? question = await _questionRepository.GetQuestionById(id) ?? throw new ObjectNotFoundException("question", "No questions found with that ID");
            return await Task.FromResult(question);
        }

        public async Task<Question> GetQuestion() {
            IEnumerable<Question> questions = await _questionRepository.GetAllQuestions();
            if(questions.Count() == 0) {
                throw new ObjectNotFoundException("question", "No questions found");
            }
            Question randomQuestion = questions.ElementAt(new Random().Next(questions.Count()));
            return await Task.FromResult(randomQuestion);
        }

        public async Task<Question> AddQuestion(Question question) {
            if(await _questionRepository.GetQuestionFromText(question.Text) != null) {
                throw new IllegalArgumentException("text", "A question with that text already exists");
            }
            Question newQuestion = await _questionRepository.AddQuestion(new Question(question.Text, question.Answer));
            return await Task.FromResult(newQuestion);
        }

        public async Task<Question> UpdateQuestion(Question question) {
            Question? currentQuestion = await _questionRepository.GetQuestionById(question.Id);
            if(currentQuestion == null) {
                throw new ObjectNotFoundException("question", "No questions found with that ID");
            }
            currentQuestion.UpdateText(question.Text);
            currentQuestion.UpdateAnswer(question.Answer);
            return await Task.FromResult(currentQuestion);
        }

        public async Task<Question> DeleteQuestion(Guid id) {
            Question? question = await _questionRepository.GetQuestionById(id);
            if(question == null) {
                throw new ObjectNotFoundException("question", "No questions found with that ID");
            }
            return await Task.FromResult(await _questionRepository.DeleteQuestion(id) ?? throw new ObjectNotFoundException("question", "No questions found with that ID"));
        }
    }
}