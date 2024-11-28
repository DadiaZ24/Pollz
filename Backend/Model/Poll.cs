namespace Oscars.Backend.Model
{
	public class Poll
	{
		public required int Id { get; set; }
		public required int UserId { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required DateTime Created_at { get; set; }
		public DateTime? Updated_at { get; set; }
	}
}
