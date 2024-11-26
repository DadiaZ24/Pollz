using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/category")]
    public class CategoryRouter : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryRouter(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //GET ALL CATEGORY
        [HttpGet]
        public ActionResult GetCategories()
        {
            var category = _categoryService.GetCategories();
            if (category == null)
            {
                return NotFound("No categories found");
            }
            return Ok(category);
        }

        //GET A CATEGORY
        [HttpGet("{id}")]
        public ActionResult GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(category);
        }

        //CREATE A CATEGORY
        [HttpPost]
        public ActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
        }

        //UPDATE A CATEGORY
        [HttpPut("{id}")]
        public ActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category id");
            }
            _categoryService.UpdateCategory(categoryDto);
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
            _categoryService.DeleteCategory(id);
            return NoContent();
        }

    }
}