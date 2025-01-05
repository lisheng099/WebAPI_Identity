using WebAPI.Dtos.News;
using WebAPI.Dtos.NewsFiles;
using WebAPI.Models;
    
namespace WebAPI_Identity.Mappers
{
    public static class NewsMapper
    {
        public static NewsDto NewsDataToDto(this News news)
        {
            NewsDto newsDto = new NewsDto
            {
                Title = news.Title,
                Content = news.Content,
                NewsId = news.NewsId,
                StartDateTime = news.StartDateTime,
                EndDateTime = news.EndDateTime,
                InsertEmployeeName = news.InsertEmployee.UserName,
                UpdateEmployeeName = news.UpdateEmployee.UserName,
                Click = news.Click,
                Order = news.Order
            };

            List<NewsFilesDto> files = new List<NewsFilesDto>();
            if (news.NewsFiles != null)
            {
                foreach (var item in news.NewsFiles)
                {
                    files.Add(NewsFilesDataToDto(item));
                }
            }
            newsDto.NewFilesParameters = files;
            return newsDto;
        }

        public static NewsFilesDto NewsFilesDataToDto(this NewsFiles newsFiles)
        {
            return new NewsFilesDto
            {
                NewsFilesId = newsFiles.NewsFilesId,
                Name = newsFiles.Name,
                Path = newsFiles.Path,
                Extension = newsFiles.Extension,
            };
        }
    }
}
