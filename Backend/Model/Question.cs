namespace Oscars.Backend.Model
{
    /// <summary>
    /// Represents a question in a poll.
    /// </summary>
    public class Question
    {
        public required int Id { get; set; }
        public required int pollId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
