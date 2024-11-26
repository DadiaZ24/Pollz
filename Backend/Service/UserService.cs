using System.Collections.Generic;
using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Models;

namespace Oscars.Backend.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void CreateUser(UserDto userDto);
        void UpdateUser(int id, UserDto userDto);
        void DeleteUser(int id);
    }

    public class UserService : IUserService
    {
        private readonly string _connectionString;
        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }

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