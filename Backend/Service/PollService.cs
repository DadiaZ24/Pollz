using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;
using Oscars.Backend.Utils;

namespace Oscars.Backend.Service
{
	/// <summary>
	/// Service for handling poll-related operations.
	/// </summary>
	public class PollService(string connectionString)
	{
		private readonly string _connectionString = connectionString;
		/// <summary>
		/// Creates a new poll.
		/// </summary>
		/// <param name="pollDto">The poll data transfer object containing poll details.</param>
		/// <returns>The created <see cref="Poll"/>.</returns>
		public Poll Create(PollDto pollDto)
		{
			var poll = new Poll
			{
				Id = 0,
				UserId = pollDto.UserId,
				Title = pollDto.Title,
				Description = pollDto.Description,
				Created_at = pollDto.Created_at,
				Updated_at = pollDto.Updated_at,
			};

			using var connection = new NpgsqlConnection(_connectionString);
			connection.Open();

			using var cmd = new NpgsqlCommand("INSERT INTO operations.polls (user_id, title, description) VALUES (@1, @2, @3) RETURNING *", connection);
			cmd.Parameters.AddWithValue("@1", poll.UserId);
			cmd.Parameters.AddWithValue("@2", poll.Title);
			cmd.Parameters.AddWithValue("@3", poll.Description ?? (object)DBNull.Value);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
				poll.Id = reader.GetInt32(0);

			Console.WriteLine("Poll created with id: " + poll.Id);

			return poll;
		}
		/// <summary>
		/// Gets all polls.
		/// </summary>
		/// <returns>A list of <see cref="PollDto"/>.</returns>
		public List<PollDto> GetAll()
		{
			List<PollDto> pollsDto = [];

			using var connection = new NpgsqlConnection(_connectionString);
			connection.Open();

			using var cmd = new NpgsqlCommand("SELECT * FROM operations.polls", connection);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				var pollDto = new PollDto
				{
					Id = reader.GetInt32(0),
					UserId = reader.GetInt32(1),
					Title = reader.GetString(2),
					Description = reader.IsDBNull(3) ? null : reader.GetString(3),
					Created_at = reader.GetDateTime(4),
					Updated_at = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
				};
				pollsDto.Add(pollDto);
			}

			return pollsDto;
		}
		/// <summary>
		/// Gets a poll by its ID.
		/// </summary>
		/// <param name="pollId">The poll ID.</param>
		/// <returns>A <see cref="PollDto"/> if found; otherwise, null.</returns>
		public PollDto? GetById(int pollId)
		{
			PollDto? pollDto = null;
			using var connection = new NpgsqlConnection(_connectionString);
			connection.Open();

			using var cmd = new NpgsqlCommand("SELECT * FROM operations.polls WHERE id = @1", connection);
			cmd.Parameters.AddWithValue("@1", pollId);
			using var reader = cmd.ExecuteReader();

			if (reader.Read())
			{
				var parser = new Parser();
				pollDto = new PollDto
				{
					Id = pollId,
					UserId = reader.GetInt32(1),
					Title = reader.GetString(2),
					Description = reader.GetString(2),
					Created_at = reader.GetDateTime(4),
				};
				if (reader.IsDBNull(5))
				{
					pollDto.Updated_at = null;
				}
				else
				{
					pollDto.Updated_at = reader.GetDateTime(5);
				}
			}

			return pollDto;
		}
		/// <summary>
		/// Updates an existing poll.
		/// </summary>
		/// <param name="pollDto">The poll data transfer object containing updated poll details.</param>
		/// <returns>The updated <see cref="Poll"/>.</returns>
		public Poll Update(PollDto pollDto)
		{
			var poll = new Poll
			{
				Id = pollDto.Id,
				UserId = pollDto.UserId,
				Title = pollDto.Title,
				Description = pollDto.Description,
				Created_at = pollDto.Created_at,
				Updated_at = pollDto.Updated_at,
			};

			using var connection = new NpgsqlConnection(_connectionString);
			connection.Open();

			using var cmd = new NpgsqlCommand("UPDATE operations.polls SET title = @2, description = @3, updated_at = @4 WHERE id = @1 RETURNING *", connection);
			cmd.Parameters.AddWithValue("@1", poll.Id);
			cmd.Parameters.AddWithValue("@2", poll.Title);
			cmd.Parameters.AddWithValue("@3", poll.Description ?? (object)DBNull.Value);
			cmd.Parameters.AddWithValue("@4", DateTime.Now);
			cmd.ExecuteNonQuery();

			return poll;
		}
		/// <summary>
		/// Deletes a poll by its ID.
		/// </summary>
		/// <param name="id">The poll ID.</param>
		public void Delete(int id)
		{
			using var connection = new NpgsqlConnection(_connectionString);
			connection.Open();

			using var cmd = new NpgsqlCommand("DELETE FROM operations.polls WHERE id = @1", connection);
			cmd.Parameters.AddWithValue("@1", id);
			cmd.ExecuteNonQuery();
		}
	}
}
