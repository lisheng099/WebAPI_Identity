using System.ComponentModel.DataAnnotations;
using WebAPI.Abstracts;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos
{
    public class NewsPutDto: NewsDtoAbstract
    {
        [Required]
        public Guid NewsId { get; set; }
        public Guid UpdataEmployeeId { get; set; }
    }
}
