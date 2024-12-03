using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/question")]
    public class QuestionRouter(QuestionService questionService) : ControllerBase
    {
        private readonly QuestionService _questionService = questionService;

        //GET ALL CATEGORY
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
        //GET A CATEGORY
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

        //CREATE A CATEGORY
        [HttpPost]
        public ActionResult Create([FromBody] QuestionDto questionDto)
        {
            var question = _questionService.Create(questionDto);
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }

        //UPDATE A CATEGORY
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

        //DELETE A CATEGORY
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
