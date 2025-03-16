using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    /// <summary>
    /// Service for handling voter-related operations.
    /// </summary>
    public class VoterService(string connectionString)
    {
        private readonly string _connectionString = connectionString;
        /// <summary>
        /// Creates a new voter.
        /// </summary>
        /// <param name="voterDto">The voter data transfer object containing voter details.</param>
        /// <returns>The created <see cref="Voter"/>.</returns>
        public Voter Create(VoterDto voterDto)
        {
            var uniqueCode = new UniqueCode
            {
                Id = 0,
                VoterId = 0,
                Code = "",
                Used = false
            };

            var voter = new Voter
            {
                Id = voterDto.Id,
                PollId = voterDto.PollId,
                Name = voterDto.Name,
                Email = voterDto.Email,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();


            using var voterCmd = new NpgsqlCommand("INSERT INTO operations.voters (poll_id, name) VALUES (@1, @2) RETURNING *", connection);
            voterCmd.Parameters.AddWithValue("@1", voter.PollId);
            voterCmd.Parameters.AddWithValue("@2", voter.Name);
            var result = voterCmd.ExecuteScalar();
            if (result != null)
            {
                voter.Id = (int)result;
            }
            else
            {
                throw new InvalidOperationException("Failed to insert voter and retrieve ID.");
            }

            if (voter.Id > 0)
            {
                uniqueCode.VoterId = voter.Id;
                uniqueCode.Code = uniqueCode.Generate();

                using var codeCmd = new NpgsqlCommand("INSERT INTO operations.uniquecodes (voter_id, code) VALUES (@1, @2) RETURNING *", connection);
                codeCmd.Parameters.AddWithValue("@1", uniqueCode.VoterId);
                codeCmd.Parameters.AddWithValue("@2", uniqueCode.Code);
                var codeResult = codeCmd.ExecuteScalar();
            }

            return voter;
        }
        /// <summary>
        /// Gets all voters.
        /// </summary>
        /// <returns>A list of <see cref="VoterWithCodeDto"/>.</returns>
        public List<VoterWithCodeDto> GetAll()
        {
            List<VoterWithCodeDto> votersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.id, v.poll_id, v.name, uc.code, uc.used FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var voterDto = new VoterWithCodeDto
                {
                    Id = reader.GetInt32(0),
                    PollId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Code = reader.GetString(3),
                    Used = reader.GetBoolean(4),
                };
                votersDto.Add(voterDto);
            }

            return votersDto;
        }
        /// <summary>
        /// Gets voters by poll ID.
        /// </summary>
        /// <param name="pollId">The poll ID.</param>
        /// <returns>A list of <see cref="VoterWithCodeDto"/>.</returns>
        public List<VoterWithCodeDto> GetByPoll(int pollId)
        {
            List<VoterWithCodeDto> votersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.id, v.name, uc.code, uc.used FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id WHERE v.poll_id = @1", connection);
            cmd.Parameters.AddWithValue("@1", pollId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var voterDto = new VoterWithCodeDto
                {
                    Id = reader.GetInt32(0),
                    PollId = pollId,
                    Name = reader.GetString(1),
                    Code = reader.GetString(2),
                    Used = reader.GetBoolean(3),
                };
                votersDto.Add(voterDto);
            }

            return votersDto;
        }
        /// <summary>
        /// Gets a voter by their ID.
        /// </summary>
        /// <param name="voterId">The voter ID.</param>
        /// <returns>A <see cref="VoterWithCodeDto"/> if found; otherwise, null.</returns>
        public VoterWithCodeDto? GetById(int voterId)
        {
            VoterWithCodeDto? voterDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.poll_id, v.name, uc.code, uc.used FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id WHERE v.id = @1", connection);
            cmd.Parameters.AddWithValue("@1", voterId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                voterDto = new VoterWithCodeDto
                {
                    Id = voterId,
                    PollId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Code = reader.GetString(2),
                    Used = reader.GetBoolean(3),
                };
            }

            return voterDto;
        }
        /// <summary>
        /// Gets a voter by their code.
        /// </summary>
        /// <param name="code">The voter code.</param>
        /// <returns>A <see cref="VoterWithCodeDto"/> if found; otherwise, null.</returns>
        public VoterWithCodeDto? GetVoterByCode(string code)
        {
            VoterWithCodeDto? voterDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.id, v.poll_id, v.name, uc.code, uc.used FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id WHERE uc.code = @1", connection);
            cmd.Parameters.AddWithValue("@1", code);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                voterDto = new VoterWithCodeDto
                {
                    Id = reader.GetInt32(0),
                    PollId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Code = reader.GetString(3),
                    Used = reader.GetBoolean(4),
                };
            }

            return voterDto;
        }
        /// <summary>
        /// Updates an existing voter.
        /// </summary>
        /// <param name="voterDto">The voter data transfer object containing updated voter details.</param>
        /// <returns>The updated <see cref="Voter"/>.</returns>
        public Voter Update(VoterDto voterDto)
        {
            var voter = new Voter
            {
                Id = voterDto.Id,
                PollId = voterDto.PollId,
                Name = voterDto.Name,
                Email = voterDto.Email,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE operations.voters SET name = @3 WHERE id = @1 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@2", voter.Name);
            cmd.ExecuteNonQuery();

            return voter;
        }
        /// <summary>
        /// Updates the status of a voter's code.
        /// </summary>
        /// <param name="voterWithCodeDto">The voter data transfer object containing voter details and code.</param>
        public void UpdateCodeStatus(VoterWithCodeDto voterWithCodeDto)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE operations.uniquecodes SET used = false WHERE code = @1 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@2", voterWithCodeDto.Code);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Deletes a voter by their ID.
        /// </summary>
        /// <param name="id">The voter ID.</param>
        public void Delete(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM operations.voters WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}
