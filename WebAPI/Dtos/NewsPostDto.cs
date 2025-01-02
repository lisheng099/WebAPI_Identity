using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using WebAPI.Abstracts;
using WebAPI.Models;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos
{
    public class NewsPostDto : NewsDtoAbstract
    {
        public Guid InsertEmployeeId { get; set; }
    }
}
