using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Servers;
using WebAPI_Identity.Interfaces;
using WebAPI_Identity.Servers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<WebContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebDatabase")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<WebContext>().AddDefaultTokenProviders();
//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebContext>();

builder.Services.AddScoped<INewsService, NewsServicer>();
builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = new PathString("/api/Login/NoLogin");
    options.AccessDeniedPath = new PathString("/api/Login/NoAccess");
});

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedAsync(services);
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
