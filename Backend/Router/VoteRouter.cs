using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/vote")]
    public class VoteRouter(VoteService voteService) : ControllerBase
    {
        private readonly VoteService _voteService = voteService;

        [HttpPost]
        public async Task<IActionResult> Insert(VoteRequestDto voteRequestDto)
        {
            var hasVoted = await _voteService.HasVotedAsync(voteRequestDto);

            if (hasVoted)
            {
                var updated = await _voteService.Create(voteRequestDto);
                if (updated)
                {
                    return Ok("Vote created successfully.");
                }
                else
                {
                    return BadRequest("Failed to update vote.");
                }
            }
            else
            {
                var created = await _voteService.Create(voteRequestDto);
                if (created)
                {
                    return Ok("Vote created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create vote.");
                }
            }
        }

        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetResults(int questionId)
        {
            var results = await _voteService.GetResultsAsync(questionId);

            if (results.Count == 0)
            {
                return Ok(results);
            }
            return Ok(results);
        }
    }
}
