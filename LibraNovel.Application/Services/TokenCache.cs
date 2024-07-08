using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.Wrappers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class TokenCache : ITokenCache
    {
        private readonly SemaphoreSlim _refreshTokenSemaphore = new SemaphoreSlim(1, 1);
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public async Task<Response<LoginResponse>> GetTokenAsync(Func<string, DateTime, Task<Response<LoginResponse>>> refreshTokenFunc, string refreshToken, DateTime now)
        {
            string cacheKey = "Response";
            var response = _cache.Get<LoginResponse>(cacheKey);

            // Kiểm tra cache
            if (response == null)
            {
                // Sử dụng SemaphoreSlim để đảm bảo chỉ một request được phép refresh token tại một thời điểm
                await _refreshTokenSemaphore.WaitAsync();
                try
                {
                    response = _cache.Get<LoginResponse>(cacheKey);
                    if (response == null)
                    {
                        var result = await refreshTokenFunc(refreshToken, now);
                        if (result != null && result.Succeeded)
                        {
                            response = result.Data;
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(result.Data.AccessToken.ExpireMinutes));
                            _cache.Set(cacheKey, response, cacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    _refreshTokenSemaphore.Release();
                }
            }

            return new Response<LoginResponse>
            {
                Succeeded = true,
                Data = response
            };
        }
    }
}
