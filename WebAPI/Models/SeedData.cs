using Microsoft.EntityFrameworkCore;
using System;

namespace WebAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WebContext(serviceProvider.GetRequiredService<DbContextOptions<WebContext>>()))
            {
                if (!context.Employee.Any())
                {
                    context.Employee.AddRange(
                    new Employee
                    {
                        Name = "員工1",
                    },
                    new Employee
                    {
                        Name = "員工2",
                    });
                    context.SaveChanges();
                }

                if (!context.News.Any())
                {
                    Guid guid = context.Employee.FirstOrDefault().EmployeeId;
                    context.News.AddRange(
                    new News
                    {
                        Title = "第一個新聞",
                        Content = "新聞的內容1",
                        InsertEmployeeId = guid,
                        UpdateEmployeeId = guid,
                        Click = 0,
                        Enable = true,
                        Order = 1,
                    },
                    new News
                    {
                        Title = "第二個新聞",
                        Content = "新聞的內容2",
                        InsertEmployeeId = guid,
                        UpdateEmployeeId = guid,
                        Click = 0,
                        Enable = true,
                        Order = 2,
                    }, new News
                    {
                        Title = "第三個新聞",
                        Content = "新聞的內容3",
                        InsertEmployeeId = guid,
                        UpdateEmployeeId = guid,
                        Click = 0,
                        Enable = true,
                        Order = 3,
                    });
                    context.SaveChanges();
                }

                if (context.News.Any() && !context.NewsFiles.Any())
                {
                    Guid guid = context.News.FirstOrDefault().NewsId;
                    context.NewsFiles.AddRange(
                        new NewsFiles
                        {
                            NewsId = guid,
                            Name = "第一個檔案",
                            Path = "src/file",
                            Extension = "測試",
                        },
                        new NewsFiles
                        {
                            NewsId = guid,
                            Name = "第二個檔案",
                            Path = "src/file",
                        }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
