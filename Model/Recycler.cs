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
        public static string DbConnection { get; set; }
        public Recycler(string status)
        {
            Status = status;
        }
        public Recycler()
        { }
        public void RecyclerConnection(string dbConnection)
        {
            DbConnection = dbConnection;
        }
        public async void RecyclerDeleteAsync(int days)
        {
            var videoDb = new VideoDb(DbConnection);
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var results = await query.ReadAllVideosByDaysAsync(days);

            if (results is null || results.Count == 0)
                return;

            Status = "running";
            // Thread.Sleep(10000);
            foreach (var result in results)
            {
                var filePath = "videos/" + result.ServerId + "/" + result.Id;
                //Remove video from Db
                Thread threadDb = new Thread(() => result.ThreadDeleteAsync());
                //Delete physical binary
                Thread threadIO = new Thread(() => System.IO.File.Delete(filePath));

                threadDb.Start();
                threadIO.Start();
                threadDb.Join();
                threadIO.Join();
            }
            Status = "not running";
        }
    }
}