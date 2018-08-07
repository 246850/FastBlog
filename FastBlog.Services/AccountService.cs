using Dapper;
using FastBlog.Services.Entities;
using System.Data;
using System.Linq;

namespace FastBlog.Services
{
    public interface IAccountService:IDependency
    {
        AccountEntity Login(AccountEntity entity);

        AccountEntity Get(int id);
    }
    public class AccountService : IAccountService
    {
        private readonly IDbConnection _connection;
        public AccountService(IDbConnection connection)
        {
            _connection = connection;
        }

        public AccountEntity Get(int id)
        {
            string sql = "select * from account where Id=@Id";
            return _connection.QueryFirstOrDefault<AccountEntity>(sql, new { Id = id });
        }

        public AccountEntity Login(AccountEntity entity)
        {
            string sql = "select * from account where Account=@Account and Pwd=@Pwd";
            var list = _connection.Query<AccountEntity>(sql, new
            {
                entity.Account,
                entity.Pwd
            }).ToList();

            if (list != null && list.Count == 1) return list[0];

            return null;
        }
    }
}
