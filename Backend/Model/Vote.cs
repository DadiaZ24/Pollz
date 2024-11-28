namespace Oscars.Backend.Model
{
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
