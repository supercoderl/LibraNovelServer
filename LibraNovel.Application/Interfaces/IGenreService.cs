using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Genre;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IGenreService
    {
        Task<Response<GenreResponse>> GetGenreByID(int genreID);
        Task<Response<RequestParameter<GenreResponse>>> GetAllGenres(int pageIndex, int pageSize);
        Task<Response<string>> CreateGenre(CreateGenreViewModel request);
        Task<Response<string>> UpdateGenre(int genreID, UpdateGenreViewModel request);
        Task<Response<string>> DeleteGenre(int genreID);
    }
}
