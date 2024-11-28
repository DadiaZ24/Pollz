using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
	[ApiController]
	[Route("api/polls")]
	public class PollRouter(PollService pollService) : ControllerBase
	{
		private readonly PollService _pollService = pollService;

		//GET ALL Answer
		[HttpGet]
		public ActionResult GetAll()
		{
			var poll = _pollService.GetAll();
			if (poll == null)
			{
				return NotFound("No polls found");
			}
			return Ok(poll);
		}

		//GET A Answer
		[HttpGet("{id}")]
		public ActionResult GetById(int id)
		{
			var poll = _pollService.GetById(id);
			if (poll == null)
			{
				return NotFound("Poll not found");
			}
			return Ok(poll);
		}

		//CREATE A Answer
		[HttpPost]
		public ActionResult Create([FromBody] PollDto pollDto)
		{
			_pollService.Create(pollDto);
			return CreatedAtAction(nameof(GetById), new { id = pollDto.Id }, pollDto);
		}

		//UPDATE A Answer
		[HttpPut("{id}")]
		public ActionResult Update(int id, [FromBody] PollDto pollDto)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid poll id");
			}
			_pollService.Update(pollDto);
			return NoContent();
		}

		//DELETE A Answer
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid poll id");
			}
			_pollService.Delete(id);
			return NoContent();
		}

	}
}
