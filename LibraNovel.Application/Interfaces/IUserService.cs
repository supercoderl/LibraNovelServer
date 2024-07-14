using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.ViewModels.UsersRoles;
using LibraNovel.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IUserService
    {
        Task<Response<LoginResponse>> Login(LoginViewModel request, string ipAddress);
        Task<Response<LoginResponse>> LoginByProvider(LoginProviderViewModel request, string ipAddress);
        Task<Response<string>> Register(IFormFile? file, RegisterViewModel request);
        Task<Response<string>> RevokeToken(string token);
        Task<Response<RequestParameter<UserInformation>>> GetAllUsers(int pageIndex, int pageSize);
        Task<Response<List<UserInformation>>> GetUserByIDs(List<Guid> userIDs);
        Task<Response<UserInformation>> GetUserByIDORCode(Guid? userID, string? code);
        Task<Response<string>> UpdateInformation(Guid userID, IFormFile? file, UpdateUserViewModel request);
        Task<Response<string>> ChangePassword(Guid userID, ChangePasswordViewModel request);
        Task<Response<string>> DeleteUser(Guid userID);
        Task<Response<string>> UpdateAvatar(Guid userID, IFormFile file);
        Task<Response<string>> SendResetCode(ResetPasswordEmail request);
        Task<Response<string>> VerifyResetCode(VerifyResetPasswordCode request);
        Task<Response<LoginResponse>> GetTokenAsync(string refreshToken, DateTime now);
        Task<Response<string>> CreateMappingUserWithRoles(CreateUsersRolesViewModel request);
    }
}
