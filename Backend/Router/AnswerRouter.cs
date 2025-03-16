using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/answer")]
    public class AnswerRouter(AnswerService answerService) : ControllerBase
    {
        private readonly AnswerService _answerService = answerService;

        /// <summary>
        /// Handles HTTP GET requests to retrieve all answers.
        /// </summary>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the list of all answers if found; 
        /// otherwise, a <see cref="Microsoft.AspNetCore.Mvc.NotFoundResult"/> result with a message indicating no answers were found.
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            var answer = _answerService.GetAll();
            if (answer == null)
            {
                return NotFound("No answers found");
            }
            return Ok(answer);
        }
        /// <summary>
        /// Retrieves answers by the specified question ID.
        /// </summary>
        /// <param name="questionId">The ID of the question to retrieve answers for.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the answers for the specified question ID.
        /// Returns a 404 Not Found response if no answers are found.
        /// </returns>
        [HttpGet("question/{questionId}")]
        public ActionResult GetByQuestionId(int questionId)
        {
            var answer = _answerService.GetByQuestionId(questionId);
            if (answer == null)
            {
                return NotFound("No answers found");
            }
            return Ok(answer);
        }
        /// <summary>
        /// Handles HTTP GET requests to retrieve an answer by its ID.
        /// </summary>
        /// <param name="id">The ID of the answer to retrieve.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the answer if found, or a NotFound result if the answer does not exist.
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var answer = _answerService.GetById(id);
            if (answer == null)
            {
                return NotFound("Answer not found");
            }
            return Ok(answer);
        }
        /// <summary>
        /// Handles HTTP POST requests to create a new answer.
        /// </summary>
        /// <param name="answerDto">The data transfer object containing the answer details.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> that contains the created answer and a location header with the URL to retrieve the answer by its ID.
        /// </returns>
        [HttpPost]
        public ActionResult Create([FromBody] AnswerDto answerDto)
        {
            var answer = _answerService.Create(answerDto);
            return CreatedAtAction(nameof(GetById), new { id = answer.Id }, answer);
        }
        /// <summary>
        /// Updates an existing answer with the specified id.
        /// </summary>
        /// <param name="id">The id of the answer to update.</param>
        /// <param name="answerDto">The data transfer object containing the updated answer details.</param>
        /// <returns>
        /// Returns a <see cref="ActionResult"/> indicating the result of the update operation.
        /// If the id is invalid, returns a <see cref="Microsoft.AspNetCore.Mvc.BadRequestResult"/> with an error message.
        /// If the update is successful, returns a <see cref="Microsoft.AspNetCore.Mvc.NoContentResult"/>.
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] AnswerDto answerDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid answer id");
            }
            _answerService.Update(answerDto);
            return NoContent();
        }
        /// <summary>
        /// Deletes an answer with the specified id.
        /// </summary>
        /// <param name="id">The id of the answer to delete.</param>
        /// <returns>
        /// Returns a <see cref="ActionResult"/>. If the id is invalid, returns a <see cref="Microsoft.AspNetCore.Mvc.BadRequestResult"/> with an error message.
        /// If the deletion is successful, returns a <see cref="Microsoft.AspNetCore.Mvc.NoContentResult"/>.
        /// </returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid answer id");
            }
            _answerService.Delete(id);
            return NoContent();
        }

    }
}
