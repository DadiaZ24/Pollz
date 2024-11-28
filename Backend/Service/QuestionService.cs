using System;
using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class QuestionService
    {
        private string _connectionString;
        public QuestionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //CREATE A CATEGORY
        public Question Create(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = questionDto.Id,
                Poll_id = questionDto.Poll_id,
                Title = questionDto.Title,
                Description = questionDto.Description,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO management.categories (name, description, nominees) VALUES (@1, @2, @3) RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", question.Title != null ? question.Title : DBNull.Value);
            cmd.Parameters.AddWithValue("@2", question.Description != null ? question.Description : DBNull.Value);
            cmd.Parameters.AddWithValue("@3", question.Poll_id);
            cmd.ExecuteNonQuery();


            return question;
        }

        //READ ALL CATEGORIES
        public List<QuestionDto> GetAll()
        {
            List<QuestionDto> questionsDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.questions", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var questionDto = new QuestionDto
                {
                    Id = reader.GetInt32(0),
                    Poll_id = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                };
                questionsDto.Add(questionDto);
            }

            return questionsDto;
        }

        //READ A CATEGORY
        public QuestionDto? GetById(int questionId)
        {
            QuestionDto? questionDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.questions WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", questionId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                questionDto = new QuestionDto
                {
                    Id = questionId,
                    Poll_id = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                };
            }

            return questionDto;
        }

        //UPDATE A CATEGORY
        public Question Update(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = questionDto.Id,
                Title = questionDto.Title,
                Description = questionDto.Description,
                Poll_id = questionDto.Poll_id,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE operations.questions SET title = @3, description = @4 WHERE id = @1 AND Poll_id = @2 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", question.Id);
            cmd.Parameters.AddWithValue("@2", question.Poll_id);
            cmd.Parameters.AddWithValue("@3", question.Title);
            cmd.Parameters.AddWithValue("@4", question.Description);
            cmd.ExecuteNonQuery();

            return question;
        }

        //DELETE A CATEGORY
        public void Delete(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM operations.questions WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}