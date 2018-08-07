using Dapper;
using FastBlog.Models;
using FastBlog.Services.Entities;
using System.Collections.Generic;
using System.Data;

namespace FastBlog.Services
{
    public interface IImageService:IDependency
    {
        int Add(ImageEntity entity);
        int Delete(int id);
        PagedList<ImageEntity> GetPagedList(int page, int pageSize, int categoryId);
        bool ExistByCategoryId(int categoryId);
        long GetCountByCategoryId(int categoryId);
    }

    public class ImageService : IImageService
    {
        private readonly IDbConnection _connection;
        public ImageService(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Add(ImageEntity entity)
        {
            string sql = @"INSERT INTO image (CategoryId, FilePath, CreateTime)
                        VALUES (@CategoryId, @FilePath, @CreateTime)";
            return _connection.Execute(sql, entity);
        }

        public int Delete(int id)
        {
            string sql = @"delete from image where Id = @Id";
            return _connection.Execute(sql, new { Id = id });
        }

        public bool ExistByCategoryId(int categoryId)
        {
            long count = GetCountByCategoryId(categoryId);
            return count > 0;
        }

        public long GetCountByCategoryId(int categoryId)
        {
            string sql = "select count(0) from image where CategoryId = @CategoryId";
            return _connection.ExecuteScalar<long>(sql, new { CategoryId = categoryId });
        }

        public PagedList<ImageEntity> GetPagedList(int page, int pageSize, int categoryId)
        {
            //  总记录数
            string countSql = "select count(0) from image";
            // 查询列表
            string sql = "select * from image";
            string where = string.Empty;
            if (categoryId > 0) // 根据分类查询
            {
                where += $" where CategoryId ={categoryId}";
                countSql += where;
                sql += where;
            }
            sql += " order by Id Desc limit @Row, @PageSize"; // 分页
            long total = _connection.ExecuteScalar<long>(countSql);//  总记录数
            IEnumerable<ImageEntity> list = _connection.Query<ImageEntity>(sql, new { Row = (page - 1) * pageSize, PageSize = pageSize });

            return list.ToPaged(page, pageSize, total);
        }
    }
}
