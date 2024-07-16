using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Genre;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public GenreService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Create new genre
        public async Task<Response<string>> CreateGenre(CreateGenreViewModel request)
        {
            var genre = _mapper.Map<Genre>(request);
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo thể loại thành công", null);
        }

        //Delete genre
        public async Task<Response<string>> DeleteGenre(int genreID)
        {
            var genre = await _context.Genres.FindAsync(genreID);
            if(genre == null)
            {
                throw new ApiException("Thể loại không tồn tại");
            }
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa thể loại thành công", null);
        }

        //Get genres list
        public async Task<Response<RequestParameter<GenreResponse>>> GetAllGenres(int pageIndex, int pageSize)
        {
            var genres = await _context.Genres.OrderBy(g => g.GenreID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return new Response<RequestParameter<GenreResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<GenreResponse>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = genres.Count(),
                    Items = genres.Select(g => _mapper.Map<GenreResponse>(g)).ToList(),
                }
            };
        }

        //Get single genre by id
        public async Task<Response<GenreResponse>> GetGenreByID(int genreID)
        {
            var genre = await _context.Genres.FindAsync(genreID);
            if (genre == null)
            {
                throw new ApiException("Thể loại không tồn tại");
            }
            return new Response<GenreResponse>(_mapper.Map<GenreResponse>(genre), null);
        }

        //Update genre
        public async Task<Response<string>> UpdateGenre(int genreID, UpdateGenreViewModel request)
        {
            if(genreID != request.GenreID)
            {
                throw new ApiException("Thể loại không hợp lệ");
            }

            var genre = await _context.Genres.FindAsync(genreID);
            if (genre == null)
            {
                throw new ApiException("Thể loại không tồn tại");
            }

            _mapper.Map(request, genre);
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }
    }
}
