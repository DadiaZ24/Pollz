using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{

	//AUTHENTICATION
	[ApiController]
	[Route("api/auth")]
	public class LoginRouter : ControllerBase
	{
		private readonly IAuthService _authService;
		public LoginRouter(IAuthService authService)
		{
			_authService = authService;
		}

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
