using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using EveIndyBoss.Infrastructure;

namespace EveIndyBoss.Services
{
    public interface IProvideStaticData
    {
        Task<IEnumerable<Cat>> Test();
    }

    public class StaticDataService : IProvideStaticData
    {
        private readonly Func<IDbConnection> _connectionCreator;

        public StaticDataService(Func<IDbConnection> connectionCreator)
        {
            _connectionCreator = connectionCreator;
        }

        public async Task<IEnumerable<Cat>> Test()
        {
            using (var conn = _connectionCreator())
            {
                conn.EnsureOpenConnection();

                var result = await conn.QueryAsync<Cat>(@"SELECT Id, Name FROM InventoryCategories");
                return result;
            }
        }
    }

    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}