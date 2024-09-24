using JavaHateBE.exceptions;
using JavaHateBE.model;
using JavaHateBE.service;
using Microsoft.AspNetCore.Mvc;

namespace JavaHateBE.controller
{
    [ApiController]
    [Route("question")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;

        private readonly QuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, QuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
        {
            try
            {
                IEnumerable<Question> questions = await _questionService.GetAllQuestions();
                return Ok(questions);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new { message = e.Message, entity = e.Object });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Question>> GetQuestionById([FromRoute] Guid id)
        {
            try
            {
                Question question = await _questionService.GetQuestionById(id);
                return Ok(question);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get question by id.");
                return NotFound(new { message = e.Message, entity = e.Object });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get question by id.");
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("random")]
        public async Task<ActionResult<Question>> GetRandomQuestion()
        {
            try
            {
                Question question = await _questionService.GetQuestion();
                return Ok(question);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get random question.");
                return NotFound(new { message = e.Message, entity = e.Object });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get random question.");
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Question>> AddQuestion([FromBody] Question question)
        {
            try
            {
                Question addedQuestion = await _questionService.AddQuestion(question);
                return CreatedAtAction(nameof(GetQuestionById), new { id = addedQuestion.Id }, addedQuestion);
            }
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, "Failed to add question.");
                return BadRequest(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add question.");
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Question>> UpdateQuestion([FromBody] Question question)
        {
            try
            {
                Question updatedQuestion = await _questionService.UpdateQuestion(question);
                return Ok(updatedQuestion);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to update question.");
                return NotFound(new { message = e.Message, entity = e.Object });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to update question.");
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult> DeleteQuestion([FromRoute] Guid id)
        {
            try
            {
                await _questionService.DeleteQuestion(id);
                return NoContent();
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to delete question.");
                return NotFound(new { message = e.Message, entity = e.Object });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete question.");
                return BadRequest(new { message = e.Message });
            }
        }
    }
}