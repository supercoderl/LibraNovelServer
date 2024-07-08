using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Bookmark;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class BookmarkController : BaseApiController
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet("/get-all-bookmarks")]
        [Authorize]
        public async Task<IActionResult> GetAllBookmarks(int pageIndex = 1, int pageSize = 10)
        {
            return Ok(await _bookmarkService.GetAllBookmarks(pageIndex, pageSize)); 
        }

        [HttpPost("/create-bookmark")]
        [Authorize]
        public async Task<IActionResult> CreateBookmark(CreateBookmarkViewModel request)
        {
            return Ok(await _bookmarkService.CreateBookmark(request));  
        }

        [HttpDelete("/delete-bookmark/{bookmarkID}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookmark(int bookmarkID)
        {
            return Ok(await _bookmarkService.DeleteBookmark(bookmarkID));   
        }
    }
}
