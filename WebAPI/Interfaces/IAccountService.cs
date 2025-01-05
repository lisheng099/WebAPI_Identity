using System.Security.Claims;
using WebAPI_Identity.Dtos.Login;

namespace WebAPI_Identity.Interfaces
{
    public interface IAccountService
    {
        public Task<ClaimsPrincipal> Lonin(LoginPostDto value);
        public Task Logout();
    }
}
