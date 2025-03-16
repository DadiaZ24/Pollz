using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    /// <summary>
    /// Router for handling vote-related requests.
    /// </summary>
    [ApiController]
    [Route("api/vote")]
    public class VoteRouter(VoteService voteService) : ControllerBase
    {
        private readonly VoteService _voteService = voteService;
        /// <summary>
        /// Inserts a new vote or updates an existing vote.
        /// </summary>
        /// <param name="voteRequestDto">The vote request data transfer object containing vote details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the vote insertion or update.</returns>
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
        /// <summary>
        /// Gets the results of a vote by question ID.
        /// </summary>
        /// <param name="questionId">The question ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vote results.</returns>
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
