using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    /// <summary>
    /// Router for handling question-related requests.
    /// </summary>
    [ApiController]
    [Route("api/question")]
    public class QuestionRouter(QuestionService questionService) : ControllerBase
    {
        private readonly QuestionService _questionService = questionService;
        /// <summary>
        /// Gets all questions.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the list of questions.</returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            var question = _questionService.GetAll();
            if (question == null)
            {
                return NotFound("No question found");
            }
            return Ok(question);
        }
        /// <summary>
        /// Gets questions by poll ID.
        /// </summary>
        /// <param name="pollId">The poll ID.</param>
        /// <returns>An <see cref="ActionResult"/> containing the questions.</returns>
        [HttpGet("poll/{pollId}")]
        public ActionResult GetByPollId(int pollId)
        {
            var question = _questionService.GetByPollId(pollId);
            if (question == null)
            {
                return NotFound("No question found");
            }
            return Ok(question);
        }
        /// <summary>
        /// Gets a question by its ID.
        /// </summary>
        /// <param name="id">The question ID.</param>
        /// <returns>An <see cref="ActionResult"/> containing the question.</returns>
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var question = _questionService.GetById(id);
            if (question == null)
            {
                return NotFound("Question not found");
            }
            return Ok(question);
        }
        /// <summary>
        /// Creates a new question.
        /// </summary>
        /// <param name="questionDto">The question data transfer object containing question details.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the creation.</returns>
        [HttpPost]
        public ActionResult Create([FromBody] QuestionDto questionDto)
        {
            var question = _questionService.Create(questionDto);
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }
        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <param name="id">The question ID.</param>
        /// <param name="questionDto">The question data transfer object containing updated question details.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the update.</returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] QuestionDto questionDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category id");
            }
            _questionService.Update(questionDto);
            return NoContent();
        }
        /// <summary>
        /// Deletes a question by its ID.
        /// </summary>
        /// <param name="id">The question ID.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the deletion.</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category id");
            }
            _questionService.Delete(id);
            return NoContent();
        }

    }
}
