namespace Oscars.Backend.Dtos
{
    public class VoterDto
    {
        public required int Id { get; set; }
        /// <summary>
        /// Represents a Data Transfer Object (DTO) for a voter in the PollZ platform.
        /// </summary>
        public required int PollId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
    }
    /// <summary>
    /// Represents a unique code assigned to a voter.
    /// </summary>
    public class UniqueCodeDto
    {
        public required int Id { get; set; }
        public required int VoterId { get; set; }
        public required string Code { get; set; }
        public required bool Used { get; set; }
    }
    /// <summary>
    /// Represents a voter with an associated code in the polling system.
    /// </summary>
    public class VoterWithCodeDto
    {
        public required int Id { get; set; }
        public required int PollId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Code { get; set; }
        public required bool Used { get; set; }
    }
}
