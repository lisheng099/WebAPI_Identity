using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Parameters
{
    public class NewsFilesIdParameter
    {
        [Required]
        public Guid NewsId { get; set; }
        [Required]
        public Guid NewsFilesId { get; set; }
    }
}
