using System.ComponentModel.DataAnnotations;
using WebAPI.Dtos.News;
using WebAPI.Dtos.NewsFiles;
using WebAPI.Models;

namespace WebAPI.Abstracts
{
    public abstract class NewsDtoAbstract : IValidatableObject
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public int Click { get; set; }
        [Required]
        public bool Enable { get; set; }
        [Required]
        public int Order { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            WebContext _webContext = (WebContext)validationContext.GetService(typeof(WebContext));

            var findTitle = from a in _webContext.News
                            where a.Title == Title
                            select a;

            if(this.GetType() == typeof(NewsFilesPutDto))
            {
                var dtoUpdate = (NewsPutDto)this;
                findTitle = findTitle.Where(x => x.NewsId != dtoUpdate.NewsId);
            }

            if (findTitle.FirstOrDefault() != null)
            {
                yield return new ValidationResult("已存在相同的新聞名稱");
            }

            if (StartDateTime >= EndDateTime)
            {
                yield return new ValidationResult("開始時間不可大於結束時間");
            }
        }
    }
}
