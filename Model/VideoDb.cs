using System;
using MySqlConnector;

namespace VideoMonitoramento
{
    public class VideoDb : IDisposable
    {
        public MySqlConnection Connection { get; }

        public VideoDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}