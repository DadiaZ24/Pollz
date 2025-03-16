using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Models;

namespace Oscars.Backend.Service
{
    /// <summary>
    /// Interface for user service.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void CreateUser(UserDto userDto);
        void UpdateUser(int id, UserDto userDto);
        void DeleteUser(int id);
    }
    /// <summary>
    /// Service for handling user-related operations.
    /// </summary>
    public class UserService(string connectionString) : IUserService
    {
        private readonly string _connectionString = connectionString;
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="User"/>.</returns>
        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand("SELECT * FROM management.users", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Role = reader.GetString(4)
                });
            }
            return users;
        }
        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A <see cref="User"/> if found; otherwise, throws <see cref="KeyNotFoundException"/>.</returns>
        public User GetUserById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand("SELECT * FROM management.users WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Role = reader.GetString(4)
                };
            }
            throw new KeyNotFoundException("User not found");
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing user details.</param>
        public void CreateUser(UserDto userDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand("INSERT INTO management.users (username, email, password, role) VALUES (@1, @2, @3, @4)", connection);
            cmd.Parameters.AddWithValue("@1", userDto.Username);
            cmd.Parameters.AddWithValue("@2", userDto.Email);
            cmd.Parameters.AddWithValue("@3", userDto.Password);
            cmd.Parameters.AddWithValue("@4", userDto.Role.ToString());
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="userDto">The user data transfer object containing updated user details.</param>
        public void UpdateUser(int id, UserDto userDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand("UPDATE management.users SET username = @1, email = @2, password = @3, role = @4 WHERE id = @5", connection);
            cmd.Parameters.AddWithValue("@1", userDto.Username);
            cmd.Parameters.AddWithValue("@2", userDto.Email);
            cmd.Parameters.AddWithValue("@3", userDto.Password);
            cmd.Parameters.AddWithValue("@4", userDto.Role);
            cmd.Parameters.AddWithValue("@5", id);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        public void DeleteUser(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand("DELETE FROM management.users WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}