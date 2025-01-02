using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Servers
{
    public class NewsServer(WebContext webContext) : INewsServer
    {
        private readonly WebContext _webContext = webContext;

        public List<NewsDto> GetNewsByParameter(NewsSelectParameter parameter)
        {
            var result = _webContext.News
                .Include(x => x.InsertEmployee)
                .Include(x => x.UpdateEmployee)
                .Include(x=>x.NewsFiles)
                .Select(x => x);

            if (!string.IsNullOrWhiteSpace(parameter.Title))
            {
                result = result.Where(a => a.Title == parameter.Title);
            }

            if (!string.IsNullOrWhiteSpace(parameter.Content))
            {
                result = result.Where(a => a.Content.Contains(parameter.Content));
            }

            if (parameter.StartDateTime != null)
            {
                result = result.Where(a => a.StartDateTime.Date == ((DateTime)parameter.StartDateTime).Date);
            }

            if (parameter.MinOrder != 0 && parameter.MinOrder != null)
            {
                result = result.Where(a => a.Order >= parameter.MinOrder);
            }

            if (parameter.MaxOrder != 0 && parameter.MaxOrder != null)
            {
                result = result.Where(a => a.Order <= parameter.MaxOrder);
            }

            return result.Select(x => NewsDataToDto(x)).ToList();
        }
        public NewsDto GetNewsById(Guid newsId)
        {
            var result = _webContext.News
                .Include(x => x.InsertEmployee)
                .Include(x => x.UpdateEmployee)
                .Include(x=>x.NewsFiles)
                .Where(x=> x.NewsId == newsId)
                .Select(x => x);
            return result.Select(x => NewsDataToDto(x)).SingleOrDefault();
        }
        public NewsDto PostNews(NewsPostDto value)
        {
            News insert = new News
            {
                Title = value.Title,
                Content = value.Content,
                StartDateTime = value.StartDateTime,
                EndDateTime = value.EndDateTime,
                Click = value.Click,
                Enable = value.Enable,
                Order = value.Order,
                InsertEmployeeId = value.InsertEmployeeId,
                UpdateEmployeeId = value.InsertEmployeeId,
                InsertDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
            };
            _webContext.News.Add(insert);
            _webContext.SaveChanges();
            var result = NewsDataToDto(insert); ;

            return result;
        }
        public NewsDto PutNews(Guid newsId, NewsPutDto value)
        {
            News news = (from a in _webContext.News
                         where a.NewsId == newsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return null;
            }
            else
            {
                news.UpdateEmployeeId = value.UpdataEmployeeId;
                news.UpdateDateTime = DateTime.Now;

                _webContext.News.Update(news).CurrentValues.SetValues(value);
                _webContext.SaveChanges();

                var result = NewsDataToDto(news); ;
                result.NewFilesParameters = (from a in _webContext.NewsFiles
                                             where a.NewsId == newsId
                                             select NewsFilesDataToDto(a)).ToList();
                return result;
            }
        }
        public NewsDto PatchNews(Guid newsId, JsonPatchDocument value)
        {
            News news = (from a in _webContext.News
                         where a.NewsId == newsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return null;
            }
            else
            {
                value.ApplyTo(news);
                news.UpdateEmployeeId = _webContext.Employee.FirstOrDefault().EmployeeId; ;
                news.UpdateDateTime = DateTime.Now;

                _webContext.SaveChanges();

                var result = NewsDataToDto(news); ;
                result.NewFilesParameters = (from a in _webContext.NewsFiles
                                             where a.NewsId == newsId
                                             select NewsFilesDataToDto(a)).ToList();
                return result;
            }
        }
        public bool DeleteNews(Guid newsId){
            News news = (from a in _webContext.News.Include(x=>x.NewsFiles)
                         where a.NewsId == newsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return false;
            }
            else
            {
                var newsFiles = from a in _webContext.NewsFiles 
                                 where a.NewsId == newsId
                                 select a;
                if (newsFiles != null)
                {
                    _webContext.NewsFiles.RemoveRange(newsFiles);
                    _webContext.SaveChanges();
                }

                _webContext.News.Remove(news);
                _webContext.SaveChanges();
                return true;
            }
        }

        private static NewsDto NewsDataToDto(News news)
        {
            NewsDto newsDto = new NewsDto
            {
                Title = news.Title,
                Content = news.Content,
                NewsId = news.NewsId,
                StartDateTime = news.StartDateTime,
                EndDateTime = news.EndDateTime,
                InsertEmployeeName = news.InsertEmployee.Name,
                UpdateEmployeeName = news.UpdateEmployee.Name,
                Click = news.Click,
                Order = news.Order
            };

            List<NewsFilesDto> files = new List<NewsFilesDto>();
            if(news.NewsFiles != null) { 
                foreach(var item in news.NewsFiles)
                {
                    files.Add(NewsFilesDataToDto(item));
                }
            }
            newsDto.NewFilesParameters = files;
            return newsDto;
        }


        public List<NewsFilesDto> GetNewsFilesByNewsId(Guid newsId)
        {
            var result = from a in _webContext.NewsFiles
                         where a.NewsId == newsId
                         select NewsFilesDataToDto(a);

            return result.ToList();
        }
        public NewsFilesDto GetNewsFilesByNewsId_NewsFilesId(Guid newsId, Guid newsFilesId)
        {
            var result = (from a in _webContext.NewsFiles
                         where a.NewsId == newsId && a.NewsFilesId == newsFilesId
                         select NewsFilesDataToDto(a)).SingleOrDefault();
            return result;
        }
        public NewsFilesDto PostNewsFiles(Guid newsId, NewsFilesPostDto value)
        {
            if (_webContext.News.Any(x => x.NewsId == newsId))
            {
                NewsFiles insert = new NewsFiles
                {
                    NewsId = newsId,
                    Name = value.Name,
                    Path = value.Path,
                    Extension = value.Extension,
                };
                _webContext.NewsFiles.Add(insert);
                _webContext.SaveChanges();
                return NewsFilesDataToDto(insert);
            }
            else
            {
                return null;
            }
        }
        public NewsFilesDto PutNewsFiles(Guid newsId, Guid newsFilesId, NewsFilesPutDto value)
        {
            var newsFile = (from a in _webContext.NewsFiles
                          where a.NewsId == newsId && a.NewsFilesId == newsFilesId
                          select a).SingleOrDefault();
            if (newsFile == null)
            {
                return null;
            }
            _webContext.NewsFiles.Update(newsFile).CurrentValues.SetValues(value);
            _webContext.SaveChanges();
            return NewsFilesDataToDto(newsFile);
        }
        public NewsFilesDto PatchNewsFiles(Guid newsId, Guid newsFilesId, JsonPatchDocument value)
        {
            var newsFile = (from a in _webContext.NewsFiles
                          where a.NewsId == newsId && a.NewsFilesId == newsFilesId
                          select a).SingleOrDefault();
            if (newsFile == null)
            {
                return null;
            }

            value.ApplyTo(newsFile);
            _webContext.SaveChanges();
            return NewsFilesDataToDto(newsFile);
        }
        public bool DeleteNewsFiles(Guid newsId, Guid newsFilesId)
        {
            var newsFile = (from a in _webContext.NewsFiles
                            where a.NewsId == newsId && a.NewsFilesId == newsFilesId
                            select (a)).SingleOrDefault();
            if (newsFile == null)
            {
                return false; 
            }
            else
            {
                _webContext.Remove(newsFile);
                _webContext.SaveChanges();
                return true;
            }
        }
        private static NewsFilesDto NewsFilesDataToDto(NewsFiles newsFiles)
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
