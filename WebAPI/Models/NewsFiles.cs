using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

public partial class NewsFiles
{
    public Guid NewsFilesId { get; set; }
    [ForeignKey("News")]
    public Guid NewsId { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public virtual News News { get; set; }
}
