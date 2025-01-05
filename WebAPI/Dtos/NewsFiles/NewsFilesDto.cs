namespace WebAPI.Dtos.NewsFiles
{
    public class NewsFilesDto
    {
        public Guid NewsFilesId { get; set; }
        public string Name { get; set; } = null!;

        public string Path { get; set; } = null!;

        public string Extension { get; set; } = null!;
    }
}
