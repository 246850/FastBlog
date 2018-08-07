using FastBlog.Services.Entities;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace FastBlog.Services
{
    public interface ICategoryService: IDependency
    {
        CategoryEntity Get(int id);
        List<CategoryEntity> GetAll();
        int Add(CategoryEntity entity);
        int Update(CategoryEntity entity);
        int Delete(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IDbConnection _connection;
        public CategoryService(IDbConnection connection)
        {
            _connection = connection;
        }
        public int Add(CategoryEntity entity)
        {
            string sql = @"INSERT INTO category (Title, `Desc`, Sequence, CreateTime)
                        VALUES (@Title, @Desc, @Sequence, @CreateTime)";
            return _connection.Execute(sql, entity);
        }

        public int Delete(int id)
        {
            string sql = @"delete from category where Id = @Id";
            return _connection.Execute(sql, new { Id = id });
        }

        public CategoryEntity Get(int id)
        {
            string sql = "select * from category where Id=@Id";
            return _connection.QueryFirst<CategoryEntity>(sql, new { Id = id });
        }

        public List<CategoryEntity> GetAll()
        {
            string sql = "select * from category order by Sequence desc, Id desc";
            return _connection.Query<CategoryEntity>(sql).ToList();
        }

        public int Update(CategoryEntity entity)
        {
            string sql = @"update category set Title = @Title, `Desc` = @Desc, Sequence = @Sequence where Id = @Id";
            return _connection.Execute(sql, entity);
        }
    }
}
