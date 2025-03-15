using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/voters")]
    public class VoterRouter(VoterService voterService) : ControllerBase
    {
        private readonly VoterService _voterService = voterService;

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

        [HttpGet("code/{code}")]
        public ActionResult UpdateCodeStatus(VoterWithCodeDto voterWithCodeDto)
        {
            _voterService.UpdateCodeStatus(voterWithCodeDto);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromBody] VoterDto voterDto)
        {
            var voter = _voterService.Create(voterDto);
            return CreatedAtAction(nameof(GetById), new { id = voter.Id }, voter);
        }

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

        [HttpGet("voter/{code}")]
        public ActionResult<VoterWithCodeDto> GetByCode(string code)
        {
            var voter = _voterService.getVoterByCode(code);
            if (voter == null)
            {
                return NotFound("Voter not found");
            }
            return Ok(voter);
        }

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
