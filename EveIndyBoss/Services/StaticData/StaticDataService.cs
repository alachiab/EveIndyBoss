using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EveIndyBoss.Infrastructure;
using EveIndyBoss.Models.StaticData;

namespace EveIndyBoss.Services.StaticData
{
    public interface IProvideStaticData
    {
        Task<IEnumerable<Blueprint>> GetBlueprintsAsync(int groupId);
        Task<IEnumerable<BlueprintGroup>> GetBlueprintGroupsAsync();
        Task<IEnumerable<MaterialsForType>> GetMaterialsForTypeAsync(int typeId, int activityId);
        Task<ItemLine> GetSingeItemAsync(int typeId);
    }

    public class StaticDataService : IProvideStaticData
    {
        private readonly Func<IDbConnection> _connectionCreator;

        public StaticDataService(Func<IDbConnection> connectionCreator)
        {
            _connectionCreator = connectionCreator;
        }

        public async Task<IEnumerable<Blueprint>> GetBlueprintsAsync(int groupId)
        {
            using (var conn = _connectionCreator())
            {
                conn.EnsureOpenConnection();

                return await conn.QueryAsync<Blueprint>(StaticDataSql.SelectAllBlueprints, new
                {
                    groupId
                });
            }
        }

        public async Task<IEnumerable<BlueprintGroup>> GetBlueprintGroupsAsync()
        {
            using (var conn = _connectionCreator())
            {
                conn.EnsureOpenConnection();

                return await conn.QueryAsync<BlueprintGroup>(StaticDataSql.SelectAllBlueprintGroups);
            }
        }

        public async Task<IEnumerable<MaterialsForType>> GetMaterialsForTypeAsync(int typeId, int activityId)
        {
            using (var conn = _connectionCreator())
            {
                conn.EnsureOpenConnection();

                return await conn.QueryAsync<MaterialsForType>(StaticDataSql.SelectMaterialsForType, new
                {
                    typeId,
                    activityId
                });
            }
        }

        public async Task<ItemLine> GetSingeItemAsync(int typeId)
        {
            using (var conn = _connectionCreator())
            {
                conn.EnsureOpenConnection();

                var result = await conn.QueryAsync<ItemLine>(StaticDataSql.SelectItemFromTypes, new
                {
                    typeId
                });

                var asList = result.ToList();

                return asList.Any()
                    ? asList.First()
                    : new ItemLine();
            }
        }
    }
}