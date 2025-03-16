using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Oscars.Data
{
    /// <summary>
    /// Class for managing database connections and running migrations.
    /// </summary>
    public class DBConnection(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        /// <returns>An <see cref="IDbConnection"/> object.</returns>
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString);
        }
        /// <summary>
        /// Runs database migrations asynchronously.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="scriptPath">The path to the SQL script file.</param>
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
