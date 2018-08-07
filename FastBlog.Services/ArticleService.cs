using Dapper;
using FastBlog.Models;
using FastBlog.Services.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FastBlog.Services
{
    public interface IArticleService : IDependency
    {
        ArticleEntity Get(int id);
        /// <summary>
        /// 获取上一篇
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleEntity GetPrevById(int id);
        /// <summary>
        /// 获取下一篇
        /// </summary>
        /// <param name="">id</param>
        /// <returns></returns>
        ArticleEntity GetNextById(int id);
        int Add(ArticleEntity entity);
        int Update(ArticleEntity entity);
        int Delete(int id);
        bool ExistByCategoryId(int categoryId);

        PagedList<ArticleEntity> GetPagedList(int page, int pageSize, int categoryId);

        long GetCountByCategoryId(int categoryId);
    }


    public class ArticleService : IArticleService
    {
        private readonly IDbConnection _connection;
        public ArticleService(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Add(ArticleEntity entity)
        {
            string sql = @"INSERT INTO article (AccountId, CategoryId, Title, Content, CreateTime)
                        VALUES (@AccountId, @CategoryId, @Title, @Content, @CreateTime)";
            return _connection.Execute(sql, entity);
        }

        public int Delete(int id)
        {
            string sql = @"delete from article where Id = @Id";
            return _connection.Execute(sql, new { Id = id });
        }

        public bool ExistByCategoryId(int categoryId)
        {
            long count = GetCountByCategoryId(categoryId);
            return count > 0;
        }

        public ArticleEntity Get(int id)
        {
            string sql = "select * from article where Id=@Id";
            var list = _connection.Query<ArticleEntity>(sql, new { Id = id }).ToList();
            return list.Count == 1 ? list[0] : null;
        }

        public long GetCountByCategoryId(int categoryId)
        {
            string sql = "select count(0) from article where CategoryId = @CategoryId";
            return _connection.ExecuteScalar<long>(sql, new { CategoryId = categoryId });
        }

        public ArticleEntity GetNextById(int id)
        {
            string sql = "select * from article where id > @Id order by Id asc limit 1";
            var list = _connection.Query<ArticleEntity>(sql, new { Id = id }).ToList();
            if (list != null && list.Count == 1)
            {
                return list[0];
            }
            return null;
        }

        public PagedList<ArticleEntity> GetPagedList(int page, int pageSize, int categoryId)
        {
            //  总记录数
            string countSql = "select count(0) from article";
            string where = string.Empty;
            // 查询列表
            string sql = "select * from article";
            if (categoryId > 0) // 根据分类查询
            {
                where += $" where CategoryId ={categoryId}";
                countSql += where;
                sql += where;
            }
            sql += " order by Id desc limit @Row, @PageSize"; // 分页
            long total = _connection.ExecuteScalar<long>(countSql);//  总记录数
            IEnumerable<ArticleEntity> list = _connection.Query<ArticleEntity>(sql, new { Row = (page - 1) * pageSize, PageSize = pageSize });

            return list.ToPaged(page, pageSize, total);
        }

        public ArticleEntity GetPrevById(int id)
        {
            string sql = "select * from article where id < @Id order by Id desc limit 1";
            var list = _connection.Query<ArticleEntity>(sql, new { Id = id }).ToList();
            if (list != null && list.Count == 1)
            {
                return list[0];
            }
            return null;
        }

        public int Update(ArticleEntity entity)
        {
            string sql = @"update article set CategoryId=@CategoryId, Title = @Title, Content = @Content where Id = @Id";
            return _connection.Execute(sql, entity);
        }
    }
}
