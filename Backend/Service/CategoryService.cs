using System;
using Npgsql;
using Oscars.Backend.Dtos;
using Oscars.Backend.Model;

namespace Oscars.Backend.Service
{
    public class CategoryService
    {
        private string _connectionString;
        public CategoryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //CREATE A CATEGORY
        public Category CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Nominees = categoryDto.Nominees,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO management.categories (name, description, nominees) VALUES (@1, @2, @3) RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", category.Name != null ? (object)category.Name : DBNull.Value);
            cmd.Parameters.AddWithValue("@2", category.Description != null ? (object)category.Description : DBNull.Value);
            cmd.Parameters.AddWithValue("@3", category.Nominees != null ? category.Nominees.ToArray() : DBNull.Value);
            cmd.ExecuteNonQuery();


            return category;
        }

        //READ ALL CATEGORIES
        public List<CategoryDto> GetCategories()
        {
            List<CategoryDto> categoriesDto = new List<CategoryDto>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM management.categories", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var categoryDto = new CategoryDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Nominees = reader.GetFieldValue<string[]>(3).ToList()
                };
                categoriesDto.Add(categoryDto);
            }

            return categoriesDto;
        }

        //READ A CATEGORY
        public CategoryDto? GetCategoryById(int categoryId)
        {
            CategoryDto? categoryDto = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM management.categories WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", categoryId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                categoryDto = new CategoryDto
                {
                    Id = categoryId,
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Nominees = reader.GetFieldValue<string[]>(3).ToList()
                };
            }

            return categoryDto;
        }

        //UPDATE A CATEGORY
        public Category UpdateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Nominees = categoryDto.Nominees,
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("UPDATE management.categories SET name = @2, description = @3, nominees = @4 WHERE id = @1 RETURNING *", connection);
            cmd.Parameters.AddWithValue("@1", category.Id);
            cmd.Parameters.AddWithValue("@2", category.Name);
            cmd.Parameters.AddWithValue("@3", category.Description);
            cmd.Parameters.AddWithValue("@4", category.Nominees);
            cmd.ExecuteNonQuery();

            return category;
        }

        //DELETE A CATEGORY
        public void DeleteCategory(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM management.categories WHERE id = @1", connection);
            cmd.Parameters.AddWithValue("@1", id);
            cmd.ExecuteNonQuery();
        }
    }
}