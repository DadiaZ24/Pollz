namespace Oscars.Backend.Model
{
    /// <summary>
    /// Represents a vote in the polling system.
    /// </summary>
    /// <remarks>
    /// The <see cref="Vote"/> class contains information about a vote, including the IDs of the vote, answer, question, and voter,
    /// as well as timestamps for when the vote was created and last updated.
    /// </remarks>
    public class Vote
    {
        public required int Id { get; set; }
        public required int AnswerId { get; set; }
        public required int QuestionId { get; set; }
        public required int VoterId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
