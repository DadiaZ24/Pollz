using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
	/// <summary>
	/// Router for handling poll-related requests.
	/// </summary>
	[ApiController]
	[Route("api/polls")]
	public class PollRouter(PollService pollService) : ControllerBase
	{
		private readonly PollService _pollService = pollService;
		/// <summary>
		/// Gets all polls.
		/// </summary>
		/// <returns>An <see cref="ActionResult"/> containing the list of polls.</returns>
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
		/// <summary>
		/// Gets a poll by its ID.
		/// </summary>
		/// <param name="id">The poll ID.</param>
		/// <returns>An <see cref="ActionResult"/> containing the poll.</returns>
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
		/// <summary>
		/// Creates a new poll.
		/// </summary>
		/// <param name="pollDto">The poll data transfer object containing poll details.</param>
		/// <returns>An <see cref="ActionResult"/> indicating the result of the creation.</returns>
		[HttpPost]
		public ActionResult Create([FromBody] PollDto pollDto)
		{
			var poll = _pollService.Create(pollDto);
			return CreatedAtAction(nameof(GetById), new { id = poll.Id }, poll);
		}
		/// <summary>
		/// Updates an existing poll.
		/// </summary>
		/// <param name="id">The poll ID.</param>
		/// <param name="pollDto">The poll data transfer object containing updated poll details.</param>
		/// <returns>An <see cref="ActionResult"/> indicating the result of the update.</returns>
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
		/// <summary>
		/// Deletes a poll by its ID.
		/// </summary>
		/// <param name="id">The poll ID.</param>
		/// <returns>An <see cref="ActionResult"/> indicating the result of the deletion.</returns>
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
