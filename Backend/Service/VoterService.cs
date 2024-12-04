using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class VoterService(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public Voter Create(VoterDto voterDto)
        {
            var uniqueCode = new UniqueCode
            {
                Id = 0,
                VoterId = 0,
                Code = ""
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

        //READ ALL CATEGORIES
        public List<VoterWithCodeDto> GetAll()
        {
            List<VoterWithCodeDto> votersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.id, v.poll_id, v.name, uc.code FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var voterDto = new VoterWithCodeDto
                {
                    Id = reader.GetInt32(0),
                    PollId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Code = reader.GetString(3),
                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                };
                votersDto.Add(voterDto);
            }

            return votersDto;
        }

        public List<VoterWithCodeDto> GetByPoll(int pollId)
        {
            List<VoterWithCodeDto> votersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.id, v.name, uc.code FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id WHERE v.poll_id = @1", connection);
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
                };
                votersDto.Add(voterDto);
            }

            return votersDto;
        }

        //READ A CATEGORY
        public VoterWithCodeDto? GetById(int voterId)
        {
            VoterWithCodeDto? voterDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT v.poll_id, v.name, uc.code FROM operations.voters v JOIN operations.uniquecodes uc ON v.id = uc.voter_id WHERE v.id = @1", connection);
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
                    Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                };
            }

            return voterDto;
        }

        //UPDATE A CATEGORY
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

        //DELETE A CATEGORY
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
