namespace Oscars.Backend.Dtos
{
	public class UserDto
	{
		public int Id { get; set; }
		public required string Username { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public required string Role { get; set; }
	}
}
