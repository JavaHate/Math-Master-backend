namespace JavaHateBE.service
{
    using System;
    using System.Collections.Generic;
    using JavaHateBE.model;
    using JavaHateBE.repository;
    using JavaHateBE.exceptions;

    /// <summary>
    /// Provides services for managing questions.
    /// </summary>
    public class QuestionService
    {
        private readonly QuestionRepository _questionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionService"/> class.
        /// </summary>
        /// <param name="questionRepository">The question repository.</param>
        public QuestionService(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        /// <summary>
        /// Gets a question by its ID.
        /// </summary>
        /// <param name="id">The ID of the question.</param>
        /// <returns>The question with the specified ID.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no question is found with the specified ID.</exception>
        public async Task<Question> GetQuestionById(Guid id)
        {
            Question? question = await _questionRepository.GetQuestionById(id) ?? throw new ObjectNotFoundException("question", "No questions found with that ID");
            return await Task.FromResult(question);
        }

        /// <summary>
        /// Retrieves a specified number of random questions from the repository.
        /// </summary>
        /// <param name="amount">The number of questions to retrieve. Defaults to 1 if not specified.</param>
        /// <returns>A list of randomly selected questions.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no questions are found in the repository.</exception>
        public async Task<List<Question>> GetQuestion(int? amount = 1)
        {
            IEnumerable<Question> questions = await _questionRepository.GetAllQuestions();
            if (questions.Count() == 0)
            {
                throw new ObjectNotFoundException("question", "No questions found");
            }
            List<Question> questionList = new List<Question>();
            Random random = new Random();
            for (int i = 0; i < amount; i++)
            {
                questionList.Add(questions.ElementAt(random.Next(questions.Count())));
            }
            return await Task.FromResult(questionList);
        }

        /// <summary>
        /// Adds a new question.
        /// </summary>
        /// <param name="question">The question to add.</param>
        /// <returns>The added question.</returns>
        /// <exception cref="IllegalArgumentException">Thrown when a question with the same text already exists.</exception>
        public async Task<Question> AddQuestion(Question question)
        {
            if (await _questionRepository.GetQuestionFromText(question.Text) != null)
            {
                throw new IllegalArgumentException("text", "A question with that text already exists");
            }
            Question newQuestion = await _questionRepository.AddQuestion(new Question(question.Text, question.Answer));
            return await Task.FromResult(newQuestion);
        }

        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <param name="question">The question to update.</param>
        /// <returns>The updated question.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no question is found with the specified ID.</exception>
        public async Task<Question> UpdateQuestion(Question question)
        {
            Question? currentQuestion = await _questionRepository.GetQuestionById(question.Id);
            if (currentQuestion == null)
            {
                throw new ObjectNotFoundException("question", "No questions found with that ID");
            }
            currentQuestion.UpdateText(question.Text);
            currentQuestion.UpdateAnswer(question.Answer);
            return await Task.FromResult(currentQuestion);
        }

        /// <summary>
        /// Deletes a question by its ID.
        /// </summary>
        /// <param name="id">The ID of the question to delete.</param>
        /// <returns>The deleted question.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no question is found with the specified ID.</exception>
        public async Task<Question> DeleteQuestion(Guid id)
        {
            Question? question = await _questionRepository.GetQuestionById(id);
            if (question == null)
            {
                throw new ObjectNotFoundException("question", "No questions found with that ID");
            }
            return await Task.FromResult(await _questionRepository.DeleteQuestion(id) ?? throw new ObjectNotFoundException("question", "No questions found with that ID"));
        }

        /// <summary>
        /// Gets all questions.
        /// </summary>
        /// <returns>All questions.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no questions are found.</exception>
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            IEnumerable<Question> questions = await _questionRepository.GetAllQuestions();
            if (questions.Count() == 0)
            {
                throw new ObjectNotFoundException("question", "No questions found");
            }
            return await Task.FromResult(questions);
        }
    }
}