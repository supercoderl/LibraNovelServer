using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibraNovel.Infrastructure.Data.Context;
using AutoMapper;
using System.Net;
using LibraNovel.Application.ViewModels.Email;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Role;
using Microsoft.AspNetCore.Http;
using LibraNovel.Application.ViewModels.UsersRoles;

namespace LibraNovel.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IRoleService _roleService;
        private readonly IImageService _imageService;
        private readonly ITokenCache _tokenCache;
        private static Random random = new Random();

        public UserService(IConfiguration configuration, LibraNovelContext context, IMapper mapper, IEmailService emailService, IRoleService roleService, IImageService imageService, ITokenCache tokenCache)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _roleService = roleService;
            _imageService = imageService;
            _tokenCache = tokenCache;
        }

        public async Task<Response<string>> ChangePassword(Guid userID, ChangePasswordViewModel request)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                throw new ApiException("Tài khoản không tồn tại");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                throw new ApiException("Mật khẩu cũ không đúng");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new Response<string>("Đổi mật khẩu thành công.", null);
        }

        public async Task<Response<string>> DeleteUser(Guid userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                throw new ApiException("Tài khoản không tồn tại");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa tài khoản thành công", null);
        }

        public async Task<Response<LoginResponse>> Login(LoginViewModel request, string ipAddress)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                throw new ApiException($"Email không tồn tại trong hệ thống.");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new ApiException("Sai mật khẩu!");
            }

            if(!user.IsActive)
            {
                throw new ApiException("Tài khoản của bạn đã bị khóa!");
            }

            var roles = await _roleService.GetMappingRoles(user.UserID);

            var claims = new List<Claim>
                {
                    new Claim("UserID", user.UserID.ToString()),
                    new Claim("Fullname", user.FirstName + " " + user.LastName),
                };

            foreach (var role in roles.Data)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleID.ToString()));
            }

            var result = await GenerateToken(user, claims, DateTime.Now);
            result.User = new UserResponse
            {
                UserID = user.UserID,
                Email = user.Email,
                FullName = string.Concat(user.LastName, " ", user.FirstName),
                Avatar = user.Avatar,
                Roles = roles.Data.Select(r => r.RoleCode).ToList(),
            };

            return new Response<LoginResponse>(result, $"Đăng nhập thành công.");
        }

        public async Task<Response<string>> Register(RegisterViewModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if(user != null)
            {
                throw new ApiException($"Email {request.Email} đã được sử dụng.");
            }
            
            request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            var checkAnyUserHaveRole = await _roleService.CheckAnyUserHaveRole();

            var newUser = _mapper.Map<User>(request);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            if (checkAnyUserHaveRole.Data)
            {
                await CreateMappingUserWithRoles(new CreateUsersRolesViewModel 
                { UserID = newUser.UserID, Roles = ["3"], CreatedDate = DateTime.Now });
            }
            else
            {
                await CreateMappingUserWithRoles(new CreateUsersRolesViewModel
                { UserID = newUser.UserID, Roles = ["1"], CreatedDate = DateTime.Now });
            }

            return new Response<string>("Đăng ký thành công", null);
        }

        public async Task<Response<string>> SendResetCode(ResetPasswordEmail request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                throw new ApiException("Email không có trong hệ thống");
            }
            var code = GenerateCode();
            var message = new Message(
                new string[] { request.Email },
                "Reset Password",
                $"<html><body>" +
                $"<p>Hi {string.Concat(user.FirstName, " ", user.LastName)},</p>" +
                $"<p>There was a request to change your password!</p>" +
                $"<p>If you did not make this request then please ignore this email.</p>" +
                $"<p>Otherwise, this is a code to access your reset password: <b>{code}</b></p>" +
                $"</body></html>"
            );

            return await _emailService.SendEmail(message);
        }
        private static string GenerateCode()
        {
            int randomNumber = random.Next(100000, 999999);
            string code = randomNumber.ToString();
            return code;
        }


        public async Task<Response<string>> UpdateInformation(Guid userID, IFormFile? file, UpdateUserViewModel request)
        {
            if(userID != request.UserID)
            {
                throw new ApiException("Người dùng không hợp lệ");
            }
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                throw new ApiException("Người dùng không tồn tại");
            }
            _mapper.Map(request, user);

            if(file != null)
            {
                var imageResult = await _imageService.UploadImage(file);

                if (imageResult.Succeeded && imageResult.Data != null)
                {
                    user.Avatar = imageResult.Data;
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }

        public Task<Response<string>> VerifyResetCode(VerifyResetPasswordCode request)
        {
            throw new NotImplementedException();
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<Response<LoginResponse>> RefreshToken(string refreshToken, DateTime now)
        {
            var refreshTokenObject = await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.IsActive && x.RevokeAt == null);
            if(refreshTokenObject != null)
            {
                var user = await _context.Users.FindAsync(refreshTokenObject.UserID);

                if(user == null)
                {
                    return new Response<LoginResponse>("Người dùng không tồn tại.");
                }

                var roles = await _roleService.GetMappingRoles(user.UserID);

                var claims = new List<Claim>
                {
                    new Claim("UserID", user.UserID.ToString() ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, string.Concat(user.LastName, " ", user.FirstName)),
                };

                foreach (var role in roles.Data)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleID.ToString()));
                }

                var result = await GenerateToken(user, claims, now);
                await RevokeToken(refreshToken);
                return new Response<LoginResponse>(result, "Làm mới token thành công.");
            }
            return new Response<LoginResponse>("Token không tồn tại.");
        }

        public async Task<Response<LoginResponse>> GetTokenAsync(string refreshToken, DateTime now)
        {
            return await _tokenCache.GetTokenAsync(RefreshToken, refreshToken, now);
        }

        private async Task<LoginResponse> GenerateToken(User user, List<Claim> claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(
                claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value
            );

            var jwtToken = new JwtSecurityToken(
                _configuration["JWT_Configuration:Issuer"],
                shouldAddAudienceClaim ? _configuration["JWT_Configuration:Audience"] : String.Empty,
                claims,
                expires: now.AddMinutes(Convert.ToDouble(_configuration["JWT_Configuration:TokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_Configuration:SecretKey"]!)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            var token = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpireMinutes = Convert.ToInt32(_configuration["JWT_Configuration:TokenExpirationMinutes"]),
                Exp = jwtToken.Payload.Expiration
            };

            var refreshToken = new TokenResponse
            {
                Token = GenerateRefreshToken(),
                ExpireMinutes = Convert.ToInt32(_configuration["JWT_Configuration:RefreshTokenExpirationDays"])
            };

            await StoreRefreshToken(refreshToken.Token, user.UserID);
            await Task.Delay(1000);

            return new LoginResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
            };
        }

        private async Task<bool> StoreRefreshToken(string token, Guid userID)
        {
            Token newToken = new Token
            {
                RefreshToken = token,
                UserID = userID,
                IsActive = true,
            };

            await _context.Tokens.AddAsync(newToken);
            _context.SaveChanges();
            return true;
        }

        private async Task<bool> RevokeToken(string token)
        {
            var item = await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshToken.Equals(token));
            if (item == null || !item.IsActive || item.RevokeAt != null)
            {
                return false;
            }

            item.IsActive = false;
            item.RevokeAt = DateTime.Now;

            _context.Tokens.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Response<RequestParameter<UserInformation>>> GetAllUsers(int pageIndex, int pageSize)
        {
            var users = await _context.Users.OrderBy(u => u.UserID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var usersDTO = users.Select(u => _mapper.Map<UserInformation>(u)).ToList();

            if(usersDTO.Any())
            {
                foreach(var user in usersDTO)
                {
                    var roles = await _roleService.GetMappingRoles(user.UserID);
                    if(roles.Succeeded)
                    {
                        user.Roles = roles.Data.Select(r => r.RoleID.ToString()).ToList();
                    }
                }
            }

            return new Response<RequestParameter<UserInformation>>
            {
                Succeeded = true,
                Data = new RequestParameter<UserInformation>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = users.Count,
                    Items = users.Select(u => _mapper.Map<UserInformation>(u)).ToList()
                },
            };
        }

        public async Task<Response<UserInformation>> GetUserByID(Guid userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if(user == null)
            {
                throw new ApiException("Người dùng không tồn tại");
            }

            var roles = await _roleService.GetMappingRoles(userID);
            var userDTO = _mapper.Map<UserInformation>(user);

            if(roles.Succeeded)
            {
                userDTO.Roles = roles.Data.Select(r => r.RoleID.ToString()).ToList();
            }

            return new Response<UserInformation>(userDTO, null);
        }

        async Task<Response<string>> IUserService.RevokeToken(string token)
        {
            var refreshToken = await _context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == token);
            if(refreshToken == null)
            {
                return new Response<string>(string.Empty);
            }
            refreshToken.RevokeAt = DateTime.Now;
            refreshToken.IsActive = false;

            _context.Tokens.Update(refreshToken);
            await _context.SaveChangesAsync();
            return new Response<string>("Đăng xuất thành công", null);
        }

        public async Task<Response<string>> UpdateAvatar(Guid userID, IFormFile file)
        {
            var user = await _context.Users.FindAsync(userID);
            if(user == null)
            {
                throw new ApiException("Người dùng không tồn tại");
            }

            var imageResult = await _imageService.UploadImage(file);
            if(imageResult != null && imageResult.Data != null)
            {
                user.Avatar = imageResult.Data;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new Response<string>(user.Avatar, null);
        }

        public async Task<Response<string>> CreateMappingUserWithRoles(CreateUsersRolesViewModel request)
        {
            //User roles = urs
            var urs = await _context.UsersRoles.Where(ur => ur.UserID == request.UserID).Select(rp => rp.RoleID).ToListAsync();

            List<int> newRoles;
            try
            {
                newRoles = request.Roles.Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                throw new ApiException("Một trong số các vai trò không phải định dạng số nguyên.");
            }

            foreach (var roleId in GetAddList(urs, newRoles))
            {
                await _context.UsersRoles.AddAsync(new UsersRole { UserID = request.UserID, RoleID = roleId, CreatedDate = DateTime.Now });
            }

            foreach (var roleId in GetRemoveList(urs, newRoles))
            {
                var userRole = await _context.UsersRoles
                                                   .FirstOrDefaultAsync(rp => rp.UserID == request.UserID && rp.RoleID == roleId);
                if (userRole != null)
                {
                    _context.UsersRoles.Remove(userRole);
                }
            }

            await _context.SaveChangesAsync();

            return new Response<string>("Liên kết người dùng và vai trò thành công", null);
        }

        private List<int> GetAddList(List<int?> oldRoles, List<int> newRoles)
        {
            List<int> toAdd = new List<int>();
            foreach (var role in newRoles)
            {
                if (!oldRoles.Contains(role))
                {
                    toAdd.Add(role);
                }
            }
            return toAdd;
        }

        private List<int> GetRemoveList(List<int?> oleRoles, List<int> newRoles)
        {
            List<int> toRemove = new List<int>();
            foreach (var role in oleRoles)
            {
                if (!newRoles.Contains((int)role!))
                {
                    toRemove.Add((int)role);
                }
            }
            return toRemove;
        }
    }
}
