using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class VoteService(string connectionString)
    {
        private readonly string _connectionString = connectionString;
        public async Task<bool> Create(VoteRequestDto voteRequestDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                using var cmd = new NpgsqlCommand("INSERT INTO operations.votes (answer_id, question_id, voter_id) VALUES (@1, @2, @3)", connection);
                cmd.Parameters.AddWithValue("@1", voteRequestDto.AnswerId);
                cmd.Parameters.AddWithValue("@2", voteRequestDto.QuestionId);
                cmd.Parameters.AddWithValue("@3", voteRequestDto.VoterId);

                var result = await cmd.ExecuteNonQueryAsync();

                using var cmd2 = new NpgsqlCommand("UPDATE operations.uniquecodes SET used = true WHERE voter_id = @1", connection);
                cmd2.Parameters.AddWithValue("@1", voteRequestDto.VoterId);
                await cmd2.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                Console.WriteLine("Made INSERT AND UPDATE");
                return result > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Update(VoteRequestDto voteRequestDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new NpgsqlCommand("UPDATE operations.votes SET answer_id = @1, updated_at = @2 WHERE question_id = @3 AND voter_id = @4", connection);
            cmd.Parameters.AddWithValue("@1", voteRequestDto.AnswerId);
            cmd.Parameters.AddWithValue("@2", DateTime.Now);
            cmd.Parameters.AddWithValue("@3", voteRequestDto.QuestionId);
            cmd.Parameters.AddWithValue("@4", voteRequestDto.VoterId);

            var result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        public async Task<bool> HasVotedAsync(VoteRequestDto voteRequestDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new NpgsqlCommand("SELECT 1 FROM operations.votes WHERE question_id = @1 AND voter_id = @2", connection);
            cmd.Parameters.AddWithValue("@1", voteRequestDto.QuestionId);
            cmd.Parameters.AddWithValue("@2", voteRequestDto.VoterId);

            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }

        public async Task<List<VoteResultDto>> GetResultsAsync(int questionId)
        {
            var results = new List<VoteResultDto>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new NpgsqlCommand("SELECT a.id, a.title, COUNT(v.id) AS vote_count FROM operations.answers a LEFT JOIN operations.votes v ON a.id = v.answer_id WHERE v.question_id = @1 GROUP BY a.id ORDER BY vote_count DESC;", connection);
            cmd.Parameters.AddWithValue("@1", questionId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new VoteResultDto
                {
                    AnswerId = reader.GetInt32(0),
                    Answer = reader.GetString(1),
                    VoteCount = reader.GetInt32(2)
                });
            }
            Console.WriteLine($"SELECT a.id, a.title, COUNT(v.id) AS vote_count FROM operations.answers a JOIN operations.votes v ON a.id = v.answer_id WHERE v.question_id = {questionId} GROUP BY a.id ORDER BY vote_count DESC");

            return results;
        }

    }
}
