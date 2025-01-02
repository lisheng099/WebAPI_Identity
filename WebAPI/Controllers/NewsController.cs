using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Parameters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(INewsServer newsServer) : ControllerBase
    {
        private readonly INewsServer _newsServer = newsServer;

        // GET: api/<NewsController>
        [HttpGet]
        public IActionResult Get([FromQuery]NewsSelectParameter parameter)
        {
            var result = _newsServer.GetNewsByParameter(parameter);

            if (result == null || result.Count() == 0)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if(Guid.TryParse(id, out Guid newsId))
            {
                var result = _newsServer.GetNewsById(newsId);
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
        

        // POST api/<NewsController>
        [HttpPost]
        public IActionResult Post([FromBody] NewsPostDto value)
        {
            var result = _newsServer.PostNews(value);

            if (result == null)
            {
                NotFound("找不到資料");
            }
            return Ok(result);
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] NewsPutDto value)
        {
            if (Guid.TryParse(id, out Guid newsId))
            {
                var result = _newsServer.PutNews(newsId, value);
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] JsonPatchDocument value)
        {
            if (Guid.TryParse(id, out Guid newsId))
            {
                var result = _newsServer.PatchNews(newsId, value);
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }


        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (Guid.TryParse(id, out var newsId))
            {
                if (_newsServer.DeleteNews(newsId))
                {
                    return NotFound("刪除失敗");
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NotFound("Id格式錯誤");
            }
        }

    }
}
