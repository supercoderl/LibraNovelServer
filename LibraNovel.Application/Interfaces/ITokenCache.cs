using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface ITokenCache
    {
        Task<Response<LoginResponse>> GetTokenAsync(Func<string, DateTime, Task<Response<LoginResponse>>> refreshTokenFunc, string refreshToken, DateTime now);
    }
}
