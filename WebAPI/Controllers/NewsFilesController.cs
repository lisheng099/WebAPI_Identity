using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.NewsFiles;
using WebAPI.Interfaces;
using WebAPI.Parameters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsFilesController(INewsService newsServer) : ControllerBase
    {
        private readonly INewsService _newsServer = newsServer;

        // GET: api/<NewsFilesController>
        [HttpGet("{NewsId}")]
        [Authorize(Roles = "User")]
        public IActionResult Get(Guid NewsId)
        {
            var result = _newsServer.GetNewsFilesByNewsId(NewsId);

            if (result == null || result.Count() == 0)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // GET api/<NewsFilesController>/5
        [HttpGet("{NewsId}/{newsFilesId}")]
        [Authorize(Roles = "User")]
        public IActionResult Get(Guid NewsId, Guid newsFilesId)
        {
            var result = _newsServer.GetNewsFilesByNewsId_NewsFilesId(NewsId, newsFilesId);

            if (result == null)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // POST api/<NewsFilesController>
        [HttpPost("{NewsId}")]
        [Authorize(Roles = "User")]
        public IActionResult Post(Guid NewsId, [FromForm] NewsFilesPostDto value)
        {
            value.NewsId = NewsId;
            var result = _newsServer.PostNewsFiles(value);
            if (result == null)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // PUT api/<NewsFilesController>/5
        [HttpPut("{NewsId}/{newsFilesId}")]
        [Authorize(Roles = "User")]
        public IActionResult Put(Guid NewsId, Guid newsFilesId, [FromForm] NewsFilesPutDto value)
        {
            value.NewsFilesIdParameter = new NewsFilesIdParameter { NewsId = NewsId, NewsFilesId = newsFilesId };

            var result = _newsServer.PutNewsFiles(value);
            if (result == null)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // DELETE api/<NewsFilesController>/5
        [HttpDelete("{NewsId}/{newsFilesId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid NewsId, Guid newsFilesId)
        {
            if (_newsServer.DeleteNewsFiles(new NewsFilesIdParameter { NewsId = NewsId, NewsFilesId = newsFilesId }))
            {
                return NoContent();
            }
            else
            {
                return NotFound("找不到資料");
            }
        }

    }
}
