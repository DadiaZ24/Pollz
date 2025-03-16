namespace Oscars.Backend.Utils
{
	/// <summary>
	/// Configuration settings for JWT (JSON Web Token).
	/// </summary>
	public class JwtSettings
	{
		public string SecretKey { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int ExpiryMinutes { get; set; }
	}
}
