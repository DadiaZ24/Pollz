namespace Oscars.Backend.Dtos
{
    /// <summary>
    /// Represents a request to cast a vote in a poll.
    /// </summary>
    public class VoteRequestDto
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int VoterId { get; set; }
    }
    /// <summary>
    /// Represents a request to cast a vote result in a poll.
    /// </summary>
    public class VoteResultDto
    {
        public int AnswerId { get; set; }
        public string? Answer { get; set; }
        public int VoteCount { get; set; }
    }
}
