using System.ComponentModel.DataAnnotations;
using WebAPI.Abstracts;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos.News
{
    public class NewsPutDto : NewsDtoAbstract
    {
        [Required]
        public Guid NewsId { get; set; }
    }
}
