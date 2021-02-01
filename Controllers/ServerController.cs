using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VideoMonitoramento.Controllers
{
    [Route("api/")]
    public class ServerController : ControllerBase
    {
        public ServerController(ServerDb Sdb)
        {
            serverDb = Sdb;
        }
        public ServerDb serverDb { get; }

        // Criar um novo servidor
        [HttpPost("server")]
        public async Task<IActionResult> CreateServer([FromBody] Server body)
        {
            await serverDb.Connection.OpenAsync();
            body.Id = Guid.NewGuid().ToString();
            body.Db = serverDb;
            await body.InsertAsync();

            // Create the directory.
            string path = "videos/" + body.Id;
            Directory.CreateDirectory(path);

            return new OkObjectResult(body);
        }

        // Remover um servidor existente
        [HttpDelete("servers/{id}")]
        public async Task<IActionResult> DeleteServer(string id)
        {
            await serverDb.Connection.OpenAsync();
            var query = new ServerQuery(serverDb);
            var result = await query.FindServerAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();

            // Delete the directory.
            string path = "videos/" + id;
            DeleteDirectory(path);
            return new OkResult();
        }

        // Recuperar um servidor existente
        [HttpGet("servers/{id}")]
        public async Task<IActionResult> GetServer(string id)
        {
            await serverDb.Connection.OpenAsync();
            var query = new ServerQuery(serverDb);
            var result = await query.FindServerAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // Checar disponibilidade de um servidor
        [HttpPost("servers/available/{id}")]
        public async Task<IActionResult> CheckAvailable([FromBody] Server body, string id)
        {
            await serverDb.Connection.OpenAsync();
            var query = new ServerQuery(serverDb);
            var result = await query.FindServerAsync(id);
            if (result is null)
                return new NotFoundResult();
            if (body.Port != result.Port || body.Ip != result.Ip)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // Listar todos os servidores
        [HttpGet("servers")]
        public async Task<IActionResult> GetAllServers()
        {
            await serverDb.Connection.OpenAsync();
            var query = new ServerQuery(serverDb);
            var result = await query.ReadAllServersAsync();
            if (result is null || result.Count == 0)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}