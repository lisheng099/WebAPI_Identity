using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebAPI.Models;
using WebAPI_Identity.Dtos.Login;
using WebAPI_Identity.Interfaces;

namespace WebAPI_Identity.Servers
{
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;


        public async Task<ClaimsPrincipal> Lonin(LoginPostDto value)
        {
            var result = await _signInManager.PasswordSignInAsync(value.Username, value.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(value.Username);
                var roles = await _userManager.GetRolesAsync(user);

                // 將角色資訊放入 ClaimsIdentity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "apiauth");
                var principal = new ClaimsPrincipal(identity);

                return principal;
            }
            else
            {
                return null;
            }
        }

        public async Task Logout()
        {
           await _signInManager.SignOutAsync();
        }
    }
}
