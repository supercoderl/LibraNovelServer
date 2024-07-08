using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Chapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class ChapterController : BaseApiController
    {
        private readonly IChapterService _chapterService;

        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [HttpGet("/get-chapters/{novelID}")]
        public async Task<IActionResult> GetChapters(int pageIndex = 1, int pageSize = 10, int? novelID = null)
        {
            return Ok(await _chapterService.GetAllChapters(pageIndex, pageSize, novelID));
        }

        [HttpGet("/get-chapter-by-id/{chapterID}")]
        public async Task<IActionResult> GetChapterByID(int chapterID)
        {
            return Ok(await _chapterService.GetChapterByID(chapterID));
        }

        [HttpPost("/create-chapter")]
        [Authorize]
        public async Task<IActionResult> CreateChapter(CreateChapterViewModel request)
        {
            return Ok(await _chapterService.CreateChapter(request));
        }

        [HttpPut("/update-chapter/{chapterID}")]
        [Authorize]
        public async Task<IActionResult> UpdateChapter(int chapterID, UpdateChapterModel request)
        {
            return Ok(await _chapterService.UpdateChapter(chapterID, request));
        }

        [HttpDelete("/delete-chapter/{chapterID}")]
        [Authorize]
        public async Task<IActionResult> DeleteChapter(int chapterID)
        {
            return Ok(await _chapterService.DeleteChapter(chapterID));  
        }
    }
}
