using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Dtos;
using WebAPI.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsFilesController(INewsServer newsServer,IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly INewsServer _newsServer = newsServer;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: api/<NewsFilesController>
        [HttpGet]
        public IActionResult Get(String Id)
        {
            if (Guid.TryParse(Id, out Guid newsId)) 
            {
                var result = _newsServer.GetNewsFilesByNewsId(newsId);

                if (result == null || result.Count() == 0)
                {
                    NotFound("找不到資料");
                }
                return Ok(result);
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        // GET api/<NewsFilesController>/5
        [HttpGet("{FilesId}")]
        public IActionResult Get(string Id, string FilesId)
        {
            if (Guid.TryParse(Id, out Guid newsId) && Guid.TryParse(FilesId, out Guid newsFilesId))
            {
                var result = _newsServer.GetNewsFilesByNewsId_NewsFilesId(newsId, newsFilesId);

                if (result == null)
                {
                    NotFound("找不到資料");
                }
                return Ok(result);
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        // POST api/<NewsFilesController>
        [HttpPost]
        public IActionResult Post(string Id, List<IFormFile> files)
        {
            if (Guid.TryParse(Id, out Guid newsId))
            {
                string rootRoot = _webHostEnvironment.ContentRootPath + @"\wwwroot\NewsFiles\" + newsId + @"\";
                if (!Directory.Exists(rootRoot))
                {
                    Directory.CreateDirectory(rootRoot);
                }
                List<NewsFilesDto> result = new List<NewsFilesDto>();
                foreach(IFormFile file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName;
                        using (var stream = System.IO.File.Create(rootRoot + fileName))
                        {
                            file.CopyTo(stream);
                            NewsFilesPostDto value = new NewsFilesPostDto{
                                Name = fileName,
                                Path = "/NewsFiles/" + newsId + "/" + fileName
                            };

                            result.Add(_newsServer.PostNewsFiles(newsId, value));
                        }
                    }
                }
                if (result == null)
                {
                    NotFound("找不到資料");
                }
                return Ok(result);
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        // PUT api/<NewsFilesController>/5
        [HttpPut("{FilesId}")]
        public IActionResult Put(string Id, string FilesId, [FromBody] NewsFilesPutDto value)
        {
            if (Guid.TryParse(Id, out Guid newsId) && Guid.TryParse(FilesId, out Guid newsFilesId))
            {
                var result = _newsServer.PutNewsFiles(newsId, newsFilesId, value);
                if (result == null)
                {
                    NotFound("找不到資料");
                }
                return Ok(result);
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        // PATCH api/<NewsFilesController>/5
        [HttpPatch("{FilesId}")]
        public IActionResult Patch(string Id, string FilesId, [FromBody] JsonPatchDocument value)
        {
            if (Guid.TryParse(Id, out Guid newsId) && Guid.TryParse(FilesId, out Guid newsFilesId))
            {
                var result = _newsServer.PatchNewsFiles(newsId, newsFilesId, value);
                if (result == null)
                {
                    NotFound("找不到資料");
                }
                return Ok(result);
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        // DELETE api/<NewsFilesController>/5
        [HttpDelete("{FilesId}")]
        public IActionResult Delete(string Id, string FilesId)
        {
            if (Guid.TryParse(Id, out Guid newsId) && Guid.TryParse(FilesId, out Guid newsFilesId))
            {
                if (_newsServer.DeleteNewsFiles(newsId, newsFilesId))
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("找不到資料");
                }
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }


    }
}
