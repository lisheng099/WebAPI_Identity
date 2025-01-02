using WebAPI.Abstracts;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos
{
    public class NewsDto : NewsDtoAbstract
    {
        public Guid NewsId { get; set; }

        public string InsertEmployeeName { get; set; }
        public string UpdateEmployeeName { get; set; }


        public List<NewsFilesDto> NewFilesParameters { get; set; }
    }
}
