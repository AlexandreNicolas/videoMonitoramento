using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class Video
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public long SizeInBytes { get; set; }
        public string ServerId { get; set; }
        public string Date { get; set; }

        internal VideoDb Db { get; set; }

        public Video()
        {
        }

        internal Video(VideoDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Video` (`Id`, `Description`, `SizeInBytes`, `ServerId`) VALUES (@id, @description, @sizeInBytes, @serverId);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Video` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public void ThreadDeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Video` WHERE `Id` = @id;";
            BindId(cmd);
            cmd.ExecuteNonQuery();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = Description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sizeInBytes",
                DbType = DbType.String,
                Value = SizeInBytes,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@serverId",
                DbType = DbType.String,
                Value = ServerId,
            });
        }
    }
}