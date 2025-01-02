using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Interfaces
{
    public interface INewsServer
    {
        List<NewsDto> GetNewsByParameter(NewsSelectParameter parameter);
        NewsDto GetNewsById(Guid newsId);
        NewsDto PostNews(NewsPostDto value);
        NewsDto PutNews(Guid newsId, NewsPutDto value);
        NewsDto PatchNews(Guid newsId, JsonPatchDocument value);


        bool DeleteNews(Guid newsId);

        List<NewsFilesDto> GetNewsFilesByNewsId(Guid newsId);
        NewsFilesDto GetNewsFilesByNewsId_NewsFilesId(Guid newsId, Guid newsFilesId);
        NewsFilesDto PostNewsFiles(Guid newsId, NewsFilesPostDto value);
        NewsFilesDto PutNewsFiles(Guid newsId, Guid newsFilesId, NewsFilesPutDto value);
        NewsFilesDto PatchNewsFiles(Guid newsId, Guid newsFilesId, JsonPatchDocument value);
        bool DeleteNewsFiles(Guid newsId, Guid newsFilesId);
    }
}
