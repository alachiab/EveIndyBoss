using System.Data;

namespace EveIndyBoss.Infrastructure
{
    public static class DbConnectionExtension
    {
        public static void EnsureOpenConnection(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
    }
}