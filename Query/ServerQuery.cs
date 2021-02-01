using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class ServerQuery
    {
        public ServerDb Db { get; }

        public ServerQuery(ServerDb db)
        {
            Db = db;
        }

        public async Task<List<Server>> ReadAllServersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Name`, `Ip`, `Port` FROM `Server`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        public async Task<Server> FindServerAsync(string id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Name`, `Ip`, `Port` FROM `Server` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Server>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Server>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Server(Db)
                    {
                        Id = reader.GetGuid(0).ToString(),
                        Name = reader.GetString(1),
                        Ip = reader.GetString(2),
                        Port = reader.GetInt16(3),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}