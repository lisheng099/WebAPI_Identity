using WebAPI.Abstracts;
using WebAPI.Dtos.NewsFiles;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos.News
{
    public class NewsDto : NewsDtoAbstract
    {
        public Guid NewsId { get; set; }

        public string InsertEmployeeName { get; set; }
        public string UpdateEmployeeName { get; set; }


        public List<NewsFilesDto> NewFilesParameters { get; set; }
    }
}
