using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System;

namespace VideoMonitoramento.Controllers
{
    [Route("api/recycler")]
    public class RecyclerController : ControllerBase
    {
        // Reciclar vídeos antigos
        [HttpPost("process/{days}")]
        public async Task<IActionResult> CreateRecycler(int days)
        {
            Recycler recycler = new Recycler();
            Thread thread = new Thread(() => recycler.RecyclerDeleteAsync(days));
            thread.Start();

            return new StatusCodeResult(202);
        }

        // Status de Reciclagem
        [HttpGet("status")]
        public async Task<IActionResult> GetStatusRecycler()
        {
            var status = JsonConvert.SerializeObject(new { status = Recycler.Status });
            var results = await Task.Run(() =>
                new OkObjectResult(status)
            );
            return results;
        }
    }
}