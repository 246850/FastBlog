using Dapper;
using FastBlog.Services.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FastBlog.Services
{
    public interface IImageCategoryService:IDependency
    {
        int Add(ImageCategoryEntity entity);
        int Update(ImageCategoryEntity entity);
        ImageCategoryEntity Get(int id);
        int Delete(int id);
        List<ImageCategoryEntity> GetAll();
    }

    public class ImageCategoryService : IImageCategoryService
    {
        private readonly IDbConnection _connection;
        public ImageCategoryService(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Add(ImageCategoryEntity entity)
        {
            string sql = @"INSERT INTO imagecategory (Title, `Desc`, Sequence, CreateTime)
                        VALUES (@Title, @Desc, @Sequence, @CreateTime)";
            return _connection.Execute(sql, entity);
        }

        public int Delete(int id)
        {
            string sql = @"delete from imagecategory where Id = @Id";
            return _connection.Execute(sql, new { Id = id });
        }

        public ImageCategoryEntity Get(int id)
        {
            string sql = "select * from imagecategory where Id=@Id";
            return _connection.QueryFirst<ImageCategoryEntity>(sql, new { Id = id });
        }

        public List<ImageCategoryEntity> GetAll()
        {
            string sql = "select * from imagecategory order by Sequence desc, Id desc";
            return _connection.Query<ImageCategoryEntity>(sql).ToList();
        }

        public int Update(ImageCategoryEntity entity)
        {
            string sql = @"update imagecategory set Title = @Title, `Desc` = @Desc, Sequence = @Sequence where Id = @Id";
            return _connection.Execute(sql, entity);
        }
    }
}
