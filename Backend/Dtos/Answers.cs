namespace Oscars.Backend.Dtos
{
    public class AnswerDto
    {
        public required int Id { get; set; }
        public required int QuestionId { get; set; }
        public required string Title { get; set; }
    }
}
