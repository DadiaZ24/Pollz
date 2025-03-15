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
        [HttpPost]
        public ActionResult Create([FromBody] AnswerDto answerDto)
        {
            var answer = _answerService.Create(answerDto);
            return CreatedAtAction(nameof(GetById), new { id = answer.Id }, answer);
        }
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
