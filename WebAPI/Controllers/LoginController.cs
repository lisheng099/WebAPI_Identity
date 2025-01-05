using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Identity.Dtos.Login;
using WebAPI_Identity.Interfaces;

namespace WebAPI_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController(
        IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost]
        public async Task <IActionResult> Login(LoginPostDto value)
        {
            var principal = await _accountService.Lonin(value);
            if (principal != null)
            {
                await HttpContext.SignInAsync(principal);
                return Ok("登入成功");
            }
            else
            {
                return NotFound("登入失敗。") ;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return Ok("已登出");
        }

        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }

        [HttpGet("NoAccess")]
        public string noAccess()
        {
            return "沒有權限";
        }
    }
}
