using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models;

public partial class ApplicationUser : IdentityUser
{
    public virtual ICollection<News> NewsInsertEmployees { get; set; }
    public virtual ICollection<News> NewsUpdateEmployees { get; set; }
}
