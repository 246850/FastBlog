using FastBlog.Services;
using FastBlog.Services.Entities;
using MySql.Data.MySqlClient;
using System;

namespace FastBlog.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new MySqlConnection("server=127.0.0.1;user id=root;password=123456; database=blogdb; charset=utf8");
            try
            {
                connection.Open();
                CategoryService service = new CategoryService(connection);
                int i = service.Update(new CategoryEntity()
                {
                    Id = 1,
                    Title = ".Net Core1",
                    Desc = ".net core, .net stardard技术学习"
                });
                Console.WriteLine($"成功:{i}");
                i = service.Delete(3);
                Console.WriteLine($"成功:{i}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Message={ex.Message}");
            }
            finally
            {
                connection.Clone();
                connection.Dispose();
            }
            Console.ReadKey();
        }
    }
}
