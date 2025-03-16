namespace Oscars.Backend.Model
{
    /// <summary>
    /// Represents an answer to a question in the PollZ platform.
    /// </summary>
    public class Answer
    {
        public required int Id { get; set; }
        public required int QuestionId { get; set; }
        public required string Title { get; set; }
    }
}
