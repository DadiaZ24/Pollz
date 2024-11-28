using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/answers")]
    public class AnswerRouter(AnswerService answerService) : ControllerBase
    {
        private readonly AnswerService _answerService = answerService;

        //GET ALL NOMINEE
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

        //GET A NOMINEE
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

        //CREATE A NOMINEE
        [HttpPost]
        public ActionResult Create([FromBody] AnswerDto answerDto)
        {
            _answerService.Create(answerDto);
            return CreatedAtAction(nameof(GetById), new { id = answerDto.Id }, answerDto);
        }

        //UPDATE A NOMINEE
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

        //DELETE A NOMINEE
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
