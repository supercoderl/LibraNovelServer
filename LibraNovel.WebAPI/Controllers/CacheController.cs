using LibraNovel.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class CacheController : BaseApiController
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("/check")]
        public async Task<IActionResult> CheckCache()
        {
            await _cacheService.RemoveCacheKeysContaining("novels");
            return Ok();
        }
    }
}
