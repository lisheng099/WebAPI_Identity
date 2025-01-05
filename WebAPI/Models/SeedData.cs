using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebAPI.Models
{
    public class SeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WebContext(serviceProvider.GetRequiredService<DbContextOptions<WebContext>>()))
            {
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new IdentityRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.News.Any())
                {
                    string guid = context.Users.FirstOrDefault().Id;
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

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleExists = await roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var user = await userManager.FindByEmailAsync("admin@example.com");
            if (user == null)
            {
                user = new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com" };
                var result = await userManager.CreateAsync(user, "YourStrongPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
