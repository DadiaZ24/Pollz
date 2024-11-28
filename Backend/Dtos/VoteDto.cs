namespace Oscars.Backend.Dtos
{
    public class VoteRequestDto
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int VoterId { get; set; }
    }

    public class VoteResultDto
    {
        public int AnswerId { get; set; }
        public string? Answer { get; set; }
        public int VoteCount { get; set; }
    }
}
