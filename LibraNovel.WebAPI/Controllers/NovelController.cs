using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Novel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    public class NovelController : BaseApiController
    {
        private readonly INovelService _novelService;

        public NovelController(INovelService novelService)
        {
            _novelService = novelService;
        }

        [HttpGet("/get-novels")]
        public async Task<IActionResult> GetNovels(int pageIndex = 1, int pageSize = 10, int? genreID = null, bool? isOwner = null, string? searchText = null)
        {
            string? userID = null;
            if(isOwner != null && isOwner == true)
            {
                userID = User.FindFirstValue("UserID");
            }
            return Ok(await _novelService.GetNovels(pageIndex, pageSize, genreID, userID != null ? Guid.Parse(userID) : null, searchText));
        }

        [HttpGet("/get-novel-by-id/{novelID}")]
        public async Task<IActionResult> GetNovelByID(int novelID)
        {
            return Ok(await _novelService.GetNovelByID(novelID));
        }

        [HttpPost("/create-novel")]
        [Authorize]
        public async Task<IActionResult> CreateNovel(IFormFile? file, [FromForm]CreateNovelViewModel request)
        {
            var auth = User.FindFirstValue("UserID");
            if(auth != null)
            {
                request.PublisherID = Guid.Parse(auth);
            }
            return Ok(await _novelService.CreateNovel(file, request));
        }

        [HttpPost("/create-mapping-genre-with-novel")]
        [Authorize]
        public async Task<IActionResult> CreateMappingGenreWithNovel(int genreID, int novelID)
        {
            return Ok(await _novelService.CreateMappingGenreWithNovel(genreID, novelID));
        }

        [HttpPut("/update-novel/{novelID}")]
        [Authorize]
        public async Task<IActionResult> UpdateNovel(int novelID, IFormFile? file, [FromForm]UpdateNovelViewModel request)
        {
            return Ok(await _novelService.UpdateNovel(novelID, file, request));
        }

        [HttpPut("/update-count/{novelID}")]
        public async Task<IActionResult> UpdateCount(int novelID, string type)
        {
            return Ok(await _novelService.UpdateCount(novelID, type));
        }

        [HttpDelete("/permanently-delete-novel/{novelID}")]
        [Authorize]
        public async Task<IActionResult> PermanentlyDeleteNovel(int novelID)
        {
            return Ok(await _novelService.PermanentlyDelete(novelID));
        }

        [HttpDelete("/drop-mapping-genre-with-novel")]
        [Authorize]
        public async Task<IActionResult> DropMappingGenreWithNovel(int genreID, int novelID)
        {
            return Ok(await _novelService.DropMappingGenreWithNovel(genreID, novelID));
        }

        [HttpDelete("/delete-novel/{novelID}")]
        [Authorize]
        public async Task<IActionResult> DeleteNovel(int novelID)
        {
            return Ok(await _novelService.DeleteNovel(novelID));
        }
    }
}
