using System.ComponentModel.DataAnnotations;
using WebAPI.Parameters;

namespace WebAPI.Dtos.NewsFiles
{
    public class NewsFilesPutDto
    {
        public NewsFilesIdParameter NewsFilesIdParameter { get; set; }
        public string Extension { get; set; } = null!;
        public IFormFile File { get; set; }
    }
}
