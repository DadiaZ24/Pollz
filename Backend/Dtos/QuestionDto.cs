namespace Oscars.Backend.Dtos
/// <summary>
/// Represents a Data Transfer Object (DTO) for a question in a poll.
/// This class is used to transfer question data between different layers of the application.
/// </summary>
{
    public class QuestionDto
    {
        public required int Id { get; set; }
        public required int pollId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
