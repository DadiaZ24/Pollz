namespace Oscars.Backend.Dtos
{
    public class VoterDto
    {
        public required int Id { get; set; }
        public required int PollId { get; set; }
        public required string Name { get; set; }
    }

    public class UniqueCodeDto
    {
        public required int Id { get; set; }
        public required int VoterId { get; set; }
        public required string Code { get; set; }
    }

    public class VoterWithCodeDto
    {
        public required int Id { get; set; }
        public required int PollId { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}
