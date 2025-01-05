using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.NewsFiles
{
    public class NewsFilesPostDto
    {
        [Required]
        public Guid NewsId { get; set; }
        public string Extension { get; set; } = null!;
        public List<IFormFile> files { get; set; }
    }
}
