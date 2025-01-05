using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos.News;
using WebAPI.Dtos.NewsFiles;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Parameters;
using WebAPI_Identity.Mappers;

namespace WebAPI.Servers
{
    public class NewsServicer(WebContext webContext, IWebHostEnvironment webHostEnvironment) : INewsService
    {
        private readonly WebContext _webContext = webContext;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public List<NewsDto> GetNewsByParameter(NewsSelectParameter parameter)
        {
            var result = _webContext.News
                .Include(x => x.InsertEmployee)
                .Include(x => x.UpdateEmployee)
                .Include(x => x.NewsFiles)
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

            return result.Select(x => x.NewsDataToDto()).ToList();
        }
        public NewsDto GetNewsById(Guid NewsId)
        {
            var result = _webContext.News
                .Include(x => x.InsertEmployee)
                .Include(x => x.UpdateEmployee)
                .Include(x => x.NewsFiles)
                .Where(x => x.NewsId == NewsId)
                .Select(x => x);
            return result.Select(x => x.NewsDataToDto()).SingleOrDefault();
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
                InsertEmployeeId = _webContext.Users.FirstOrDefault().Id,
                UpdateEmployeeId = _webContext.Users.FirstOrDefault().Id,
                InsertDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
            };
            _webContext.News.Add(insert);
            _webContext.SaveChanges();
            var result = insert.NewsDataToDto(); ;

            return result;
        }
        public NewsDto PutNews(Guid NewsId, NewsPutDto value)
        {
            News news = (from a in _webContext.News
                         where a.NewsId == NewsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return null;
            }
            else
            {
                news.UpdateEmployeeId = _webContext.Users.FirstOrDefault().Id;
                news.UpdateDateTime = DateTime.Now;

                _webContext.News.Update(news).CurrentValues.SetValues(value);
                _webContext.SaveChanges();

                var result = news.NewsDataToDto(); ;
                result.NewFilesParameters = (from a in _webContext.NewsFiles
                                             where a.NewsId == NewsId
                                             select a.NewsFilesDataToDto()).ToList();
                return result;
            }
        }
        public NewsDto PatchNews(Guid NewsId, JsonPatchDocument value)
        {
            News news = (from a in _webContext.News
                         where a.NewsId == NewsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return null;
            }
            else
            {
                value.ApplyTo(news);
                news.UpdateEmployeeId = _webContext.Users.FirstOrDefault().Id;
                news.UpdateDateTime = DateTime.Now;

                _webContext.SaveChanges();

                var result = news.NewsDataToDto(); ;
                result.NewFilesParameters = (from a in _webContext.NewsFiles
                                             where a.NewsId == NewsId
                                             select a.NewsFilesDataToDto()).ToList();
                return result;
            }
        }
        public bool DeleteNews(Guid NewsId)
        {
            News news = (from a in _webContext.News.Include(x => x.NewsFiles)
                         where a.NewsId == NewsId
                         select a).SingleOrDefault();
            if (news == null)
            {
                return false;
            }
            else
            {
                var newsFiles = from a in _webContext.NewsFiles
                                where a.NewsId == NewsId
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


        public List<NewsFilesDto> GetNewsFilesByNewsId(Guid NewsId)
        {
            var result = from a in _webContext.NewsFiles
                         where a.NewsId == NewsId
                         select a.NewsFilesDataToDto();

            return result.ToList();
        }
        public NewsFilesDto GetNewsFilesByNewsId_NewsFilesId(Guid NewsId, Guid newsFilesId)
        {
            var result = (from a in _webContext.NewsFiles
                          where a.NewsId == NewsId && a.NewsFilesId == newsFilesId
                          select a.NewsFilesDataToDto()).SingleOrDefault();
            return result;
        }
        public List<NewsFilesDto> PostNewsFiles(NewsFilesPostDto value)
        {
            if (_webContext.News.Any(x => x.NewsId == value.NewsId))
            {
                string rootRoot = _webHostEnvironment.ContentRootPath + @"\wwwroot\NewsFiles\" + value.NewsId + @"\";
                if (!Directory.Exists(rootRoot))
                {
                    Directory.CreateDirectory(rootRoot);
                }
                List<NewsFilesDto> result = new List<NewsFilesDto>();
                foreach (IFormFile file in value.files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName;
                        using (var stream = File.Create(rootRoot + fileName))
                        {
                            file.CopyTo(stream);

                            NewsFiles insert = new NewsFiles
                            {
                                NewsId = value.NewsId,
                                Name = fileName,
                                Path = "/NewsFiles/" + value.NewsId + "/" + fileName,
                                Extension = value.Extension,
                            };
                            _webContext.NewsFiles.Add(insert);

                            result.Add(insert.NewsFilesDataToDto());
                        }
                    }
                }
                _webContext.SaveChanges();
                return result;
            }
            else
            {
                return null;
            }
        }
        public NewsFilesDto PutNewsFiles(NewsFilesPutDto value)
        {
            var newsFile = (from a in _webContext.NewsFiles
                            where a.NewsId == value.NewsFilesIdParameter.NewsId && a.NewsFilesId == value.NewsFilesIdParameter.NewsFilesId
                            select a).SingleOrDefault();
            if (newsFile == null)
            {
                return null;
            }
            _webContext.NewsFiles.Update(newsFile).CurrentValues.SetValues(value);
            _webContext.SaveChanges();
            return newsFile.NewsFilesDataToDto();
        }
        public bool DeleteNewsFiles(NewsFilesIdParameter value)
        {
            var newsFile = (from a in _webContext.NewsFiles
                            where a.NewsId == value.NewsId && a.NewsFilesId == value.NewsFilesId
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
        
    }
}
