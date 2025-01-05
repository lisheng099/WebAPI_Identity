using Microsoft.AspNetCore.JsonPatch;
using WebAPI.Dtos.News;
using WebAPI.Dtos.NewsFiles;
using WebAPI.Parameters;

namespace WebAPI.Interfaces
{
    public interface INewsService
    {
        List<NewsDto> GetNewsByParameter(NewsSelectParameter parameter);
        NewsDto GetNewsById(Guid NewsId);
        NewsDto PostNews(NewsPostDto value);
        NewsDto PutNews(Guid NewsId, NewsPutDto value);
        NewsDto PatchNews(Guid NewsId, JsonPatchDocument value);


        bool DeleteNews(Guid NewsId);

        List<NewsFilesDto> GetNewsFilesByNewsId(Guid NewsId);
        NewsFilesDto GetNewsFilesByNewsId_NewsFilesId(Guid NewsId, Guid newsFilesId);
        List<NewsFilesDto> PostNewsFiles(NewsFilesPostDto value);
        NewsFilesDto PutNewsFiles(NewsFilesPutDto value);
        bool DeleteNewsFiles(NewsFilesIdParameter value);
    }
}
