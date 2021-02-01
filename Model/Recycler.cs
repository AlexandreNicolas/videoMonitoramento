using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using System.Threading;

namespace VideoMonitoramento
{
    public class Recycler
    {
        public static string Status { get; set; }
        private readonly string _serverConection;
        public Recycler(string configuration)
        {
            Status = configuration;
        }
        public Recycler()
        { }
        public async void RecyclerDeleteAsync(int days)
        {
            var DbConnection = "Server=127.0.0.1;Port=3306;Uid='root';Password=password;Database=videoMonitoramento;Allow User Variables=True";
            var videoDb = new VideoDb(DbConnection);
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var results = await query.ReadAllVideosByDaysAsync(days);

            if (results is null || results.Count == 0)
                return;
            new Recycler("running");
            // Thread.Sleep(10000);
            foreach (var result in results)
            {
                Thread thread = new Thread(() => result.ThreadDeleteAsync());
                thread.Start();
                thread.Join();
            }
            new Recycler("not running");
        }
    }
}