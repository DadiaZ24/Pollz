using Microsoft.EntityFrameworkCore;
using Oscars.Backend.Model;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using System.Data;

namespace Oscars.Data
{
    public class DBConnection
    {
        private readonly IConfiguration _configuration;
        public DBConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString);
        }

        public static async Task RunMigrationsAsync(string connectionString, string scriptPath)
        {
            var sqlScript = await File.ReadAllTextAsync(scriptPath);

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(sqlScript, connection);
            await command.ExecuteNonQueryAsync();

            Console.WriteLine("Migrations executed successfully");
        }
    }
}