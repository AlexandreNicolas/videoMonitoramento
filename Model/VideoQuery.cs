using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class VideoQuery
    {
        public VideoDb Db { get; }

        public VideoQuery(VideoDb db)
        {
            Db = db;
        }

        public async Task<List<Video>> ReadAllVideosAsync(string serverId)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Description`, `SizeInBytes`, `Date`, `ServerId` FROM `Video` WHERE `ServerId` = @serverId";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@serverId",
                DbType = DbType.String,
                Value = serverId,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        public async Task<List<Video>> ReadAllVideosByDaysAsync(int days)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Description`, `SizeInBytes`, `Date`, `ServerId` FROM `Video` WHERE Date < date_sub(now(), interval @days day)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@days",
                DbType = DbType.Int16,
                Value = days,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        public async Task<Video> FindVideoAsync(string id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Description`, `SizeInBytes`, `Date`, `ServerId` FROM `Video` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Video>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Video>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Video(Db)
                    {
                        Id = reader.GetGuid(0).ToString(),
                        Description = reader.GetString(1),
                        SizeInBytes = reader.GetInt32(2),
                        Date = reader.GetDateTime(3).ToString(),
                        ServerId = reader.GetGuid(4).ToString(),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}