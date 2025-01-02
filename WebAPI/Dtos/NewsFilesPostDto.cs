using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class NewsFilesPostDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Path { get; set; } = null!;
        public string Extension { get; set; } = null!;
    }
}
