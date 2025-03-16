namespace Oscars.Backend.Dtos
/// <summary>
/// Represents a Data Transfer Object (DTO) for a user.
/// This class is used to transfer user data between different layers of the application.
/// </summary>
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
