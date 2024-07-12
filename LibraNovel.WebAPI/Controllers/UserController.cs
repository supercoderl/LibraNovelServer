using Azure.Core;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.ViewModels.UsersRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LibraNovel.WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            return Ok(await _userService.Register(request));
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            return Ok(await _userService.Login(request, GenerateIPAddress()));
        }

        [AllowAnonymous]
        [HttpPost("/login-by-provider")]
        public async Task<IActionResult> LoginByProvider(LoginProviderViewModel request)
        {
            return Ok(await _userService.LoginByProvider(request, GenerateIPAddress()));
        }

        [HttpPut("/change-avatar/{userID}")]
        public async Task<IActionResult> ChangeAvatar(Guid userID, IFormFile file)
        {
            return Ok(await _userService.UpdateAvatar(userID, file));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                string? ip = Request.Headers["X-Forwarded-For"];
                return ip ?? string.Empty;
            }
            else
            {
                IPAddress? ip = HttpContext.Connection.RemoteIpAddress;
                return ip != null ? ip.MapToIPv4().ToString() : string.Empty;
            }
        }

        [HttpGet("/get-all-users")]
        public async Task<IActionResult> GetAllUsers(int pageIndex = 1, int pageSize = 10)
        {
            return Ok(await _userService.GetAllUsers(pageIndex, pageSize)); 
        }

        [HttpGet("/get-user-by-id-or-code")]
        public async Task<IActionResult> GetUserByIDORCode(Guid? userID, string? code)
        {
            return Ok(await _userService.GetUserByIDORCode(userID, code));  
        }

        [HttpGet("/get-profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userID = User.FindFirstValue("UserID");
            if(userID == null)
            {
                return Unauthorized();
            }
            return Ok(await _userService.GetUserByIDORCode(Guid.Parse(userID), null));
        }

        [HttpPost("/create-mapping-user-with-roles")]
        public async Task<IActionResult> CreateMappingUserWithRoles(CreateUsersRolesViewModel request)
        {
            return Ok(await _userService.CreateMappingUserWithRoles(request));
        }

        [HttpPut("/update-information")]
        [Authorize]
        public async Task<IActionResult> UpdateInformation(UpdateUserViewModel request)
        {
            string? userID = User.FindFirstValue("UserID");
            if(userID == null)
            {
                return Unauthorized();
            }
            return Ok(await _userService.UpdateInformation(Guid.Parse(userID), null, request));
        }

        [HttpPut("/update-user/{userID}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid userID, IFormFile? file, [FromForm] UpdateUserViewModel request)
        {
            return Ok(await _userService.UpdateInformation(userID, file, request));
        }

        [HttpPut("/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {
            string? userID = User.FindFirstValue("UserID");
            if (userID == null)
            {
                return Unauthorized();
            }
            return Ok(await _userService.ChangePassword(Guid.Parse(userID), request));
        }

        [HttpDelete("/delete-user/{userID}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(Guid userID)
        {
            return Ok(await _userService.DeleteUser(userID));   
        }

        [AllowAnonymous]
        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            return Ok(await _userService.GetTokenAsync(token, DateTime.Now));
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout(string token)
        {
            return Ok(await _userService.RevokeToken(token));
        }
    }
}
