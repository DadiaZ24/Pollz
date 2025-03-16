using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
	/// <summary>
	/// Router for handling authentication-related requests.
	/// </summary>
	[EnableCors("AllowFrontend")]
	[ApiController]
	[Route("api/auth")]
	public class LoginRouter : ControllerBase
	{
		private readonly IAuthService _authService;
		/// <summary>
		/// Initializes a new instance of the <see cref="LoginRouter"/> class.
		/// </summary>
		/// <param name="authService">The authentication service.</param>
		public LoginRouter(IAuthService authService)
		{
			_authService = authService;
		}
		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="userDto">The user data transfer object containing registration details.</param>
		/// <returns>An <see cref="IActionResult"/> indicating the result of the registration.</returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserDto userDto)
		{
			var res = await _authService.RegisterAsync(userDto);
			if (res.Success)
			{
				return Ok(res);
			}
			else
			{
				return BadRequest(res.Message);
			}
		}
		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <param name="loginDto">The login data transfer object containing login details.</param>
		/// <returns>An <see cref="IActionResult"/> indicating the result of the login attempt.</returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var res = await _authService.LoginAsync(loginDto);
			if (res.Success)
			{
				return Ok(res.Token);
			}
			else
			{
				return Unauthorized(res.Message);
			}
		}

	}
}
