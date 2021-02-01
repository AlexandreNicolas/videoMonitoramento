using System;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class ServerDb : IDisposable
    {
        public MySqlConnection Connection { get; }

        public ServerDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}