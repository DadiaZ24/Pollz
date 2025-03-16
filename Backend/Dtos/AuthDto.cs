namespace Oscars.Backend.Dtos
{
	/// <summary>
	/// Data Transfer Object (DTO) for user login.
	/// </summary>
	public class LoginDto
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}
