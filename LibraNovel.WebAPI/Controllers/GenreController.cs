using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class GenreController : BaseApiController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("/get-genres")]
        public async Task<IActionResult> GetGenres(int pageIndex = 1, int pageSize = 10)
        {
            return Ok(await _genreService.GetAllGenres(pageIndex, pageSize));
        }

        [HttpGet("/get-genre-by-id/{genreID}")]
        public async Task<IActionResult> GetGenreByID(int genreID)
        {
            return Ok(await _genreService.GetGenreByID(genreID));
        }

        [Authorize]
        [HttpPost("/create-genre")]
        public async Task<IActionResult> CreateGenre(CreateGenreViewModel request)
        {
            return Ok(await _genreService.CreateGenre(request));
        }

        [Authorize]
        [HttpPut("/update-genre/{genreID}")]
        public async Task<IActionResult> UpdateGenre(int genreID, UpdateGenreViewModel request)
        {
            return Ok(await _genreService.UpdateGenre(genreID, request));
        }

        [Authorize]
        [HttpDelete("/delete-genre/{genreID}")]
        public async Task<IActionResult> DeleteGenre(int genreID)
        {
            return Ok(await _genreService.DeleteGenre(genreID));    
        }
    }
}
