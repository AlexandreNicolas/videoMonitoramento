using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class Server
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        internal ServerDb Db { get; set; }
        
        public Server()
        {
        }

        internal Server(ServerDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Server` (`Id`, `Name`, `Ip`, `Port`) VALUES (@id, @name, @ip, @port);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Server` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
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
                ParameterName = "@name",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ip",
                DbType = DbType.String,
                Value = Ip,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@port",
                DbType = DbType.String,
                Value = Port,
            });
        }
    }
}