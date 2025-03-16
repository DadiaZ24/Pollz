using Microsoft.AspNetCore.Mvc;
using Oscars.Backend.Dtos;
using Oscars.Backend.Service;

namespace Oscars.Backend.Router
{
    /// <summary>
    /// Router for handling user-related requests.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserRouter(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of users.</returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }
        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user.</returns>
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
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing user details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the creation.</returns>
        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            _userService.CreateUser(userDto);
            return Ok();
        }
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="userDto">The user data transfer object containing updated user details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update.</returns>
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
        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion.</returns>
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