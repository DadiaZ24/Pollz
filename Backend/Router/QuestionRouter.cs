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
            var category = _questionService.GetAll();
            if (category == null)
            {
                return NotFound("No categories found");
            }
            return Ok(category);
        }

        //GET A CATEGORY
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var category = _questionService.GetById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(category);
        }

        //CREATE A CATEGORY
        [HttpPost]
        public ActionResult Create([FromBody] QuestionDto questionDto)
        {
            _questionService.Create(questionDto);
            return CreatedAtAction(nameof(GetById), new { id = questionDto.Id }, questionDto);
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
