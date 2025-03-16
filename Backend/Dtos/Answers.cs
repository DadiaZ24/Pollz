namespace Oscars.Backend.Dtos
/// <summary>
/// Represents a Data Transfer Object (DTO) for an answer.
/// This class is used to transfer answer data between processes.
/// </summary>
{
    public class AnswerDto
    {
        public required int Id { get; set; }
        public required int QuestionId { get; set; }
        public required string Title { get; set; }
    }
}
