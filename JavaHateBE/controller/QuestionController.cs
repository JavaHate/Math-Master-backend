using JavaHateBE.Exceptions;
using JavaHateBE.Model;
using JavaHateBE.Service;
using Microsoft.AspNetCore.Mvc;
using JavaHateBE.Model.DTOs;

namespace JavaHateBE.Controller
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
                _logger.LogWarning(e, e.Message.ToString());
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
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
                _logger.LogWarning(e, e.Message.ToString());
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("random")]
        public async Task<ActionResult<Question>> GetRandomQuestion([FromQuery] int? amount)
        {
            try
            {
                if (amount == null)
                {
                    List<Question> questions = await _questionService.GetQuestion();
                    return Ok(questions);
                }
                else
                {
                    List<Question> questions = await _questionService.GetQuestion(amount);
                    return Ok(questions);
                }
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, e.Message.ToString());
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message }, { "field", e.Argument } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Question>> AddQuestion([FromBody] QuestionCreateInput question)
        {
            try
            {
                Question addedQuestion = await _questionService.AddQuestion(question);
                return CreatedAtAction(nameof(GetQuestionById), new { id = addedQuestion.Id }, addedQuestion);
            }
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, "Failed to add question.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message }, { "field", e.Argument } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
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
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, "Failed to add question.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message }, { "field", e.Argument } });
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, e.Message.ToString());
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult> DeleteQuestion([FromRoute] Guid id)
        {
            try
            {
                var deletedQuestion = await _questionService.DeleteQuestion(id);
                return Ok(deletedQuestion);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, e.Message.ToString());
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message.ToString());
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }
    }
}