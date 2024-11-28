namespace Oscars.Backend.Model
{
    public class Answer
    {
        public required int Id { get; set; }
        public required int QuestionId { get; set; }
        public required string Title { get; set; }
    }
}
