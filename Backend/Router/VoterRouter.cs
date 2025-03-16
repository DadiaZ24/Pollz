using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    /// <summary>
    /// Router for handling voter-related requests.
    /// </summary>
    [ApiController]
    [Route("api/voters")]
    public class VoterRouter(VoterService voterService) : ControllerBase
    {
        private readonly VoterService _voterService = voterService;
        /// <summary>
        /// Gets all voters.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the list of voters.</returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            var voter = _voterService.GetAll();
            if (voter == null)
            {
                return NotFound("No voters found");
            }
            return Ok(voter);
        }
        /// <summary>
        /// Gets voters by poll ID.
        /// </summary>
        /// <param name="pollId">The poll ID.</param>
        /// <returns>An <see cref="ActionResult"/> containing the voters.</returns>
        [HttpGet("poll/{pollId}")]
        public ActionResult GetByPoll(int pollId)
        {
            var voter = _voterService.GetByPoll(pollId);
            if (voter == null)
            {
                return NotFound("No voters found");
            }
            return Ok(voter);
        }
        /// <summary>
        /// Gets a voter by their ID.
        /// </summary>
        /// <param name="id">The voter ID.</param>
        /// <returns>An <see cref="ActionResult"/> containing the voter.</returns>
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var voter = _voterService.GetById(id);
            if (voter == null)
            {
                return NotFound("Voter not found");
            }
            return Ok(voter);
        }
        /// <summary>
        /// Updates the status of a voter by their code.
        /// </summary>
        /// <param name="voterWithCodeDto">The voter data transfer object containing voter details and code.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the update.</returns>
        [HttpGet("code/{code}")]
        public ActionResult UpdateCodeStatus(VoterWithCodeDto voterWithCodeDto)
        {
            _voterService.UpdateCodeStatus(voterWithCodeDto);
            return NoContent();
        }
        /// <summary>
        /// Creates a new voter.
        /// </summary>
        /// <param name="voterDto">The voter data transfer object containing voter details.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the creation.</returns>
        [HttpPost]
        public ActionResult Create([FromBody] VoterDto voterDto)
        {
            var voter = _voterService.Create(voterDto);
            return CreatedAtAction(nameof(GetById), new { id = voter.Id }, voter);
        }
        /// <summary>
        /// Updates an existing voter.
        /// </summary>
        /// <param name="id">The voter ID.</param>
        /// <param name="voterDto">The voter data transfer object containing updated voter details.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the update.</returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] VoterDto voterDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid voter id");
            }
            _voterService.Update(voterDto);
            return NoContent();
        }
        /// <summary>
        /// Gets a voter by their code.
        /// </summary>
        /// <param name="code">The voter code.</param>
        /// <returns>An <see cref="ActionResult"/> containing the voter.</returns>
        [HttpGet("voter/{code}")]
        public ActionResult<VoterWithCodeDto> GetByCode(string code)
        {
            var voter = _voterService.GetVoterByCode(code);
            if (voter == null)
            {
                return NotFound("Voter not found");
            }
            return Ok(voter);
        }
        /// <summary>
        /// Deletes a voter by their ID.
        /// </summary>
        /// <param name="id">The voter ID.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the deletion.</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid voter id");
            }
            _voterService.Delete(id);
            return NoContent();
        }

    }
}
