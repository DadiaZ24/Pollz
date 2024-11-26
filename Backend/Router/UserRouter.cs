using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    [ApiController]
    [Route("api/users")]
    public class UserRouter : ControllerBase
    {
        private readonly IUserService _userService;
        public UserRouter(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                return Ok(_userService.GetUserById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            _userService.CreateUser(userDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDto userDto)
        {
            try
            {
                _userService.UpdateUser(id, userDto);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}