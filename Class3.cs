using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CollegeApp.Scripts
{
    public class GroupRepository
    {
        private readonly string _connectionString;

        public GroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateGroup(string groupName)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Groups (Name) VALUES (@Name)";

            // Передаем анонимный объект
            db.Execute(sql, new { Name = groupName });
        }
    }
}
