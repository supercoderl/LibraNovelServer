using Asp.Versioning;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class RatingController : BaseApiController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("/get-all-ratings")]
        public async Task<IActionResult> GetAllRatings(int pageIndex = 1, int pageSize = 10, int? novelID = null)
        {
            return Ok(await _ratingService.GetAllRatings(pageSize, pageIndex, novelID));
        }

        [HttpPost("/create-rating")]
        [Authorize]
        public async Task<IActionResult> CreateRating(CreateRatingViewModel request)
        {
            return Ok(await _ratingService.CreateRating(request));
        }

        [HttpDelete("/delete-rating/{ratingID}")]
        [Authorize]
        public async Task<IActionResult> DeleteRating(int ratingID)
        {
            return Ok(await _ratingService.DeleteRating(ratingID));
        }
    }
}
