using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("/get-all-comments")]
        public async Task<IActionResult> GetAllComments(int pageIndex = 1, int pageSize = 10, Guid? userID = null, int? novelID = null, int? chapterID = null, bool? isOwner = null)
        {
            if (isOwner != null && isOwner == true)
            {
                var userIDClaim = User.FindFirstValue("UserID");
                if (userIDClaim != null)
                {
                    userID = Guid.Parse(userIDClaim);
                }
            }
            return Ok(await _commentService.GetAllComments(pageIndex, pageSize, userID, novelID, chapterID));
        }

        [HttpPost("/create-comment")]
        [Authorize]
        public async Task<IActionResult> CreateComment(CreateCommentViewModel request)
        {
            var userID = User.FindFirstValue("UserID");
            if (userID == null)
            {
                return Unauthorized();
            }

            request.UserID = Guid.Parse(userID);

            return Ok(await _commentService.CreateComment(request));
        }

        [HttpPut("/update-comment/{commentID}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int commentID, UpdateCommentViewModel request)
        {
            return Ok(await _commentService.UpdateComment(commentID, request));
        }

        [HttpDelete("/delete-comment/{commentID}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentID)
        {
            return Ok(await _commentService.DeleteComment(commentID));
        }
    }
}
