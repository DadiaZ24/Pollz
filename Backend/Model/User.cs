namespace Oscars.Models
{
	public class User
	{
		public int Id { get; set; }
		public required string Username { get; set; }
		public required string Email { get; set; }
		public string? Password { get; set; }
		public required string Role { get; set; }
	}
}
