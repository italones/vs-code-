using CollegeApp.Models;
using CollegeApp.Scripts;

namespace CollegeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Строка подключения (Замените на свою!)
            string connectionString = "Server=localhost;Database=CollegeDB;Trusted_Connection=True;TrustServerCertificate=True;";

            // Инициализация репозиториев
            var studentRepo = new StudentRepository(connectionString);
            var groupRepo = new GroupRepository(connectionString);

            Console.WriteLine("=== Тестирование Dapper ===");

            // 1. Добавление студента
            var newStudent = new Student
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Email = "ivan@example.com"
            };

            Console.WriteLine("Добавление студента...");
            studentRepo.CreateStudent(newStudent);
            Console.WriteLine("Студент добавлен!");

            // 2. Получение всех студентов
            Console.WriteLine("\nСписок студентов:");
            var students = studentRepo.GetAllStudents();
            foreach (var s in students)
            {
                Console.WriteLine($"ID: {s.Id} | {s.FirstName} {s.LastName} | Email: {s.Email}");
            }

            // 3. Добавление группы
            Console.WriteLine("\nДобавление группы...");
            groupRepo.CreateGroup("П-101");
            Console.WriteLine("Группа добавлена!");
        }
    }
}