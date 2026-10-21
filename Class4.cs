using Dapper;
using Microsoft.Data.SqlClient;
using CollegeApp.Models;
using System.Data;

namespace CollegeApp.Scripts
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для получения всех студентов (метод GetAllStudents)
        public List<Student> GetAllStudents()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Students";
            return db.Query<Student>(sql).ToList();
        }

        // Метод добавления данных (CreateStudent)
        public void CreateStudent(Student student)
        {
            using IDbConnection db = new SqlConnection(_connectionString);

            // Используем параметризованный запрос
            string sql = @"
                INSERT INTO Students (FirstName, LastName, Email) 
                VALUES (@FirstName, @LastName, @Email)";

            // Dapper автоматически свяжет свойства объекта student 
            // с параметрами @FirstName, @LastName, @Email
            db.Execute(sql, student);
        }
    }
}