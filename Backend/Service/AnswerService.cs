using System;
using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class AnswerService
    {
        private string _connectionString;
        public AnswerService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //CREATE A Nominee
        public Answer Create(AnswerDto answersDto)
        {
            var answer = new Answer
            {
                Id = answersDto.Id,
                QuestionId = answersDto.QuestionId,
                Title = answersDto.Title,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO operations.answers (question_id, title) VALUES (@1, @2) RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", answer.QuestionId);
            cmd.Parameters.AddWithValue("@2", answer.Title);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                answer.Id = reader.GetInt32(0);
            }


            return answer;
        }

        //READ ALL CATEGORIES
        public List<AnswerDto> GetAll()
        {
            List<AnswerDto> answersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.answers", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var answerDto = new AnswerDto
                {
                    Id = reader.GetInt32(0),
                    QuestionId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                };
                answersDto.Add(answerDto);
            }

            return answersDto;
        }

        public List<AnswerDto> GetByQuestionId(int questionId)
        {
            List<AnswerDto> answersDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.answers WHERE question_id = @1", connection);
            cmd.Parameters.AddWithValue("@1", questionId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var answerDto = new AnswerDto
                {
                    Id = reader.GetInt32(0),
                    QuestionId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                };
                answersDto.Add(answerDto);
            }

            return answersDto;
        }

        //READ A Nominee
        public AnswerDto? GetById(int answerId)
        {
            AnswerDto? answerDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.answers WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", answerId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                answerDto = new AnswerDto
                {
                    Id = answerId,
                    QuestionId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                };
            }

            return answerDto;
        }

        //UPDATE A Nominee
        public Answer Update(AnswerDto answerDto)
        {
            var answer = new Answer
            {
                Id = answerDto.Id,
                QuestionId = answerDto.QuestionId,
                Title = answerDto.Title,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE operations.answers SET Title = @2 WHERE id = @1 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", answer.Id);
            cmd.Parameters.AddWithValue("@2", answer.Title);
            cmd.ExecuteNonQuery();

            return answer;
        }

        //DELETE A Nominee
        public void Delete(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM operations.answers WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}
