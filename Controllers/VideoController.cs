using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VideoMonitoramento.Controllers
{
    [Route("api/servers/{id}/videos")]
    public class VideoController : ControllerBase
    {
        public VideoController(VideoDb Vdb)
        {
            videoDb = Vdb;
        }
        public VideoDb videoDb { get; }

        // Adicionar um novo vídeo à um servidor
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> video, String description, string id)
        {
            var saveVideo = new Video();
            long size = video.Sum(f => f.Length);

            await videoDb.Connection.OpenAsync();

            saveVideo.Id = Guid.NewGuid().ToString();
            saveVideo.Db = videoDb;
            saveVideo.ServerId = id;
            saveVideo.SizeInBytes = size;
            saveVideo.Description = description;
            saveVideo.Date = DateTime.Now.ToString();

            try
            {
                await saveVideo.InsertAsync();
                foreach (var formFile in video)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = "videos/" + id + "/" + saveVideo.Id;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                return new NotFoundResult();
            }

            // return Ok(new { count = video.Count, size });
            return new OkObjectResult(saveVideo);
        }

        // Remover um vídeo existente 
        [HttpDelete("{videoId}")]
        public async Task<IActionResult> DeleteVideo(string videoId)
        {
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var result = await query.FindVideoAsync(videoId);
            if (result is null)
                return new NotFoundResult();
            try
            {
                //Delet physical binary
                var filePath = "videos/" + result.ServerId +"/"+ videoId;
                System.IO.File.Delete(filePath);
                //Delet in data base
                await result.DeleteAsync();
            }
            catch (System.Exception)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }

        // Recuperar um video existente
        [HttpGet("{videoId}")]
        public async Task<IActionResult> GetVideo(string videoId)
        {
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var result = await query.FindVideoAsync(videoId);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // Download do video em binario
        [HttpGet("{videoId}/binary")]
        public async Task<IActionResult> GetVideoBinary(string videoId)
        {
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var result = await query.FindVideoAsync(videoId);
            if (result is null)
                return new NotFoundResult();

            // Open binary video
            byte[] fileBytes = System.IO.File.ReadAllBytes($"videos/{result.ServerId}/{videoId}");
            string fileName = videoId;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //Listar todos os videos de um servidor
        [HttpGet]
        public async Task<IActionResult> GetAllVideo(string id)
        {
            await videoDb.Connection.OpenAsync();
            var query = new VideoQuery(videoDb);
            var result = await query.ReadAllVideosAsync(id);
            if (result is null || result.Count == 0)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
    }
}