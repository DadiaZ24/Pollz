using System;
using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class NomineeService
    {
        private string _connectionString;
        public NomineeService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //CREATE A Nominee
        public Nominee CreateNominee(NomineeDto nomineeDto)
        {
            var nominee = new Nominee
            {
                Id = nomineeDto.Id,
                Name = nomineeDto.Name,
                Email = nomineeDto.Email,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO management.nominees (name, email) VALUES (@1, @2) RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", nominee.Name != null ? nominee.Name : DBNull.Value);
            cmd.Parameters.AddWithValue("@2", nominee.Email != null ? nominee.Email : DBNull.Value);
            cmd.ExecuteNonQuery();


            return nominee;
        }

        //READ ALL CATEGORIES
        public List<NomineeDto> GetNominees()
        {
            List<NomineeDto> nomineesDto = new List<NomineeDto>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM management.nominees", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var nomineeDto = new NomineeDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                };
                nomineesDto.Add(nomineeDto);
            }

            return nomineesDto;
        }

        //READ A Nominee
        public NomineeDto? GetNomineeById(int nomineeId)
        {
            NomineeDto? nomineeDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM management.nominees WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", nomineeId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                nomineeDto = new NomineeDto
                {
                    Id = nomineeId,
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                };
            }

            return nomineeDto;
        }

        //UPDATE A Nominee
        public Nominee UpdateNominee(NomineeDto nomineeDto)
        {
            var nominee = new Nominee
            {
                Id = nomineeDto.Id,
                Name = nomineeDto.Name,
                Email = nomineeDto.Email,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE management.nominees SET name = @2, email = @3 WHERE id = @1 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", nominee.Id);
            cmd.Parameters.AddWithValue("@2", nominee.Name);
            cmd.Parameters.AddWithValue("@3", nominee.Email);
            cmd.ExecuteNonQuery();

            return nominee;
        }

        //DELETE A Nominee
        public void DeleteNominee(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM management.nominees WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}
