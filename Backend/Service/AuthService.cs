using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Oscars.Backend.Utils;
using Oscars.Backend.Dtos;
using static Oscars.Backend.Service.AuthService;

namespace Oscars.Backend.Service
{
	/// <summary>
	/// Interface for authentication service.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="userDto">The user data transfer object containing registration details.</param>
		/// <returns>An <see cref="AuthResult"/> indicating the result of the registration.</returns>
		Task<AuthResult> RegisterAsync(UserDto userDto);
		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <param name="loginDto">The login data transfer object containing login details.</param>
		/// <returns>An <see cref="AuthResult"/> indicating the result of the login attempt.</returns>
		Task<AuthResult> LoginAsync(LoginDto loginDto);
	}
	/// <summary>
	/// Service for handling authentication-related operations.
	/// </summary>
	public class AuthService(string connectionString, IOptions<JwtSettings> jwtSettings) : IAuthService
	{
		private readonly string _connectionString = connectionString;
		private readonly JwtSettings _jwtSettings = jwtSettings.Value;
		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="userDto">The user data transfer object containing registration details.</param>
		/// <returns>An <see cref="AuthResult"/> indicating the result of the registration.</returns>
		public async Task<AuthResult> RegisterAsync(UserDto userDto)
		{
			var hashedPassword = HashPassword(userDto.Password);

			using var connection = new NpgsqlConnection(_connectionString);

			var cmd = connection.CreateCommand();
			cmd.CommandText = "INSERT INTO management.users (username, email, password, role) VALUES (@1, @2, @3, @4)";
			cmd.Parameters.AddWithValue("@1", userDto.Username);
			cmd.Parameters.AddWithValue("@2", userDto.Email);
			cmd.Parameters.AddWithValue("@3", hashedPassword);
			cmd.Parameters.AddWithValue("@4", userDto.Role);
			await connection.OpenAsync();
			await cmd.ExecuteNonQueryAsync();

			return new AuthResult { Success = true, Message = "User registered successfull" };
		}
		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <param name="loginDto">The login data transfer object containing login details.</param>
		/// <returns>An <see cref="AuthResult"/> indicating the result of the login attempt.</returns>
		public async Task<AuthResult> LoginAsync(LoginDto loginDto)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			var cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM management.users WHERE username = @1";
			cmd.Parameters.AddWithValue("@1", loginDto.Username);
			await connection.OpenAsync();

			var reader = await cmd.ExecuteReaderAsync();

			if (!reader.HasRows)
			{
				return new AuthResult { Success = false, Message = "Username not found" };
			}

			await reader.ReadAsync();
			var storedPasswordHash = reader.GetString(reader.GetOrdinal("password"));
			if (VerifyPassword(loginDto.Password, storedPasswordHash))
			{
				var token = GenerateJwtToken(reader.GetString(reader.GetOrdinal("username")), reader.GetInt32(reader.GetOrdinal("id")));
				return new AuthResult { Success = true, Token = token };
			}
			else
			{
				return new AuthResult { Success = false, Message = "Invalid password" };
			}
		}
		/// <summary>
		/// Hashes the password using SHA256.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hashed password.</returns>
		private static string HashPassword(string password)
		{
			var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedBytes);
		}
		/// <summary>
		/// Verifies the password against the stored password hash.
		/// </summary>
		/// <param name="password">The password to verify.</param>
		/// <param name="storedPasswordHash">The stored password hash.</param>
		/// <returns>True if the password matches the stored hash; otherwise, false.</returns>
		private static bool VerifyPassword(string password, string storedPasswordHash)
		{
			var hashedPassword = HashPassword(password);
			return hashedPassword == storedPasswordHash;
		}
		/// <summary>
		/// Generates a JWT token for the authenticated user.
		/// </summary>
		/// <param name="username">The username of the authenticated user.</param>
		/// <param name="userId">The user ID of the authenticated user.</param>
		/// <returns>The generated JWT token.</returns>
		private string GenerateJwtToken(string username, int userId)
		{
			var expiryMinutes = _jwtSettings.ExpiryMinutes;

			if (expiryMinutes <= 0)
			{
				throw new InvalidOperationException("Invalid expiry time");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, username), new Claim("Id", userId.ToString())]),
				Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		/// <summary>
		/// Represents the result of an authentication operation.
		/// </summary>
		public class AuthResult
		{
			public bool Success { get; set; }
			public string? Message { get; set; }
			public string? Token { get; set; }
		}
	}
}