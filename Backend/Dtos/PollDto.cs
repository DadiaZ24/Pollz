namespace Oscars.Backend.Dtos
/// <summary>
/// Data Transfer Object (DTO) for representing a poll.
/// This class is used to transfer poll data between different layers of the application.
/// </summary>
{
	public class PollDto
	{
		public required int Id { get; set; }
		public required int UserId { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required DateTime Created_at { get; set; }
		public DateTime? Updated_at { get; set; }
	}
}
