﻿using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    /// <summary>
    /// Service for handling question-related operations.
    /// </summary>
    public class QuestionService(string connectionString)
    {
        private readonly string _connectionString = connectionString;
        /// <summary>
        /// Creates a new question.
        /// </summary>
        /// <param name="questionDto">The question data transfer object containing question details.</param>
        /// <returns>The created <see cref="Question"/>.</returns>
        public Question Create(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = questionDto.Id,
                pollId = questionDto.pollId,
                Title = questionDto.Title,
                Description = questionDto.Description,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO operations.questions (title, description, poll_id) VALUES (@1, @2, @3) RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", question.Title != null ? question.Title : DBNull.Value);
            cmd.Parameters.AddWithValue("@2", question.Description != null ? question.Description : DBNull.Value);
            cmd.Parameters.AddWithValue("@3", question.pollId);
            var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                question.Id = reader.GetInt32(0);
            }


            return question;
        }
        /// <summary>
        /// Gets all questions.
        /// </summary>
        /// <returns>A list of <see cref="QuestionDto"/>.</returns>
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
                    pollId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                };
                questionsDto.Add(questionDto);
            }

            return questionsDto;
        }
        /// <summary>
        /// Gets questions by poll ID.
        /// </summary>
        /// <param name="pollId">The poll ID.</param>
        /// <returns>A list of <see cref="QuestionDto"/>.</returns>
        public List<QuestionDto> GetByPollId(int pollId)
        {
            List<QuestionDto> questionsDto = [];

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM operations.questions WHERE poll_id = @1", connection);
            cmd.Parameters.AddWithValue("@1", pollId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var questionDto = new QuestionDto
                {
                    Id = reader.GetInt32(0),
                    pollId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                };
                questionsDto.Add(questionDto);
            }

            return questionsDto;
        }
        /// <summary>
        /// Gets a question by its ID.
        /// </summary>
        /// <param name="questionId">The question ID.</param>
        /// <returns>A <see cref="QuestionDto"/> if found; otherwise, null.</returns>
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
                    pollId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3),
                };
            }

            return questionDto;
        }
        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <param name="questionDto">The question data transfer object containing updated question details.</param>
        /// <returns>The updated <see cref="Question"/>.</returns>
        public Question Update(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = questionDto.Id,
                Title = questionDto.Title,
                Description = questionDto.Description,
                pollId = questionDto.pollId,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE operations.questions SET title = @3, description = @4 WHERE id = @1 AND Poll_id = @2 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", question.Id);
            cmd.Parameters.AddWithValue("@2", question.pollId);
            cmd.Parameters.AddWithValue("@3", question.Title);
            cmd.Parameters.AddWithValue("@4", question.Description);
            cmd.ExecuteNonQuery();

            return question;
        }
        /// <summary>
        /// Deletes a question by its ID.
        /// </summary>
        /// <param name="id">The question ID.</param>
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
