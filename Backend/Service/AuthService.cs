using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Oscars.Backend.Configurations;
using Oscars.Backend.Dtos;
using static Oscars.Backend.Service.AuthService;

namespace Oscars.Backend.Service
{
	public interface IAuthService
	{
		Task<AuthResult> RegisterAsync(UserDto userDto);
		Task<AuthResult> LoginAsync(LoginDto loginDto);
	}

	public class AuthService : IAuthService
	{
		private readonly string _connectionString;
		private readonly JwtSettings _jwtSettings;

		public AuthService(string connectionString, IOptions<JwtSettings> jwtSettings)
		{
			_connectionString = connectionString;
			_jwtSettings = jwtSettings.Value;
		}

		public async Task<AuthResult> RegisterAsync(UserDto userDto)
		{
			//encrypt password
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
				var token = GenerateJwtToken(reader.GetString(reader.GetOrdinal("username")));
				return new AuthResult { Success = true, Token = token };
			}
			else
			{
				return new AuthResult { Success = false, Message = "Invalid password" };
			}
		}

		private string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedBytes);
		}

		private bool VerifyPassword(string password, string storedPasswordHash)
		{
			var hashedPassword = HashPassword(password);
			return hashedPassword == storedPasswordHash;
		}


		private string GenerateJwtToken(string username)
		{
			var expiryMinutes = _jwtSettings.ExpiryMinutes;
			Console.WriteLine("///////////////////\n" + expiryMinutes + "\n///////////////////");
			Console.WriteLine("///////////////////\n" + _jwtSettings.SecretKey + "\n///////////////////");
			if (expiryMinutes <= 0)
			{
				throw new InvalidOperationException("Invalid expiry time");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
				Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public class AuthResult
		{
			public bool Success { get; set; }
			public string? Message { get; set; }
			public string? Token { get; set; }
		}
	}
}