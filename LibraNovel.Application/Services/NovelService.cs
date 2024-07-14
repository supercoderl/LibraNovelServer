using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Application.Services
{
    public class NovelService : INovelService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IChapterService _chapterService;
        private readonly IImageService _imageService;

        public NovelService(LibraNovelContext context, IMapper mapper, IUserService userService, IChapterService chapterService, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _chapterService = chapterService;
            _imageService = imageService;
        }

        public async Task<Response<string>> CreateMappingGenreWithNovel(int genreID, int novelID)
        {
            var novelsgenres = new NovelGenre
            {
                GenreID = genreID,
                NovelID = novelID
            };

            await _context.AddAsync(novelsgenres);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo liên kết thành công", null);
        }

        public async Task<Response<string>> CreateNovel(IFormFile? file, CreateNovelViewModel request)
        {
            var novel = _mapper.Map<Novel>(request);
            if (file != null)
            {
                var imageResult = await _imageService.UploadImage(file);

                if (imageResult.Succeeded && imageResult.Data != null)
                {
                    novel.CoverImage = imageResult.Data;
                }
            }

            await _context.Novels.AddAsync(novel);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo thành công", null);
        }

        public async Task<Response<string>> DeleteNovel(int novelID)
        {
            var novel = await _context.Novels.FindAsync(novelID);
            if (novel == null)
            {
                throw new ApiException("Truyện này không tồn tại");
            }
            novel.DeletedDate = DateTime.Now;
            _context.Novels.Update(novel);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa truyện thành công.", null);
        }

        public async Task<Response<string>> DropMappingGenreWithNovel(int genreID, int novelID)
        {
            var novelsgenres = await _context.NovelGenres.FirstOrDefaultAsync(ng => ng.GenreID == genreID && ng.NovelID == novelID);
            if (novelsgenres == null)
            {
                throw new ApiException("Liên kết không tồn tại.");
            }
            _context.NovelGenres.Remove(novelsgenres);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa liên kết thành công", null);
        }

        public async Task<Response<NovelResponse>> GetNovelByID(int novelID)
        {
            var novel = await _context.Novels.FirstOrDefaultAsync(n => n.DeletedDate == null && n.NovelID == novelID);
            if (novel == null)
            {
                throw new ApiException("Truyện không tồn tại hoặc đã bị xóa.");
            }

            var novelMapping = _mapper.Map<NovelResponse>(novel);

            var novelsgenres = await _context.NovelGenres.Where(ng => ng.NovelID == novelMapping.NovelID).ToListAsync();

            if (novelsgenres.Any())
            {
                novelMapping.Mappings = novelsgenres;
                novelMapping.Genres = await GetGenreFromNovel(novelsgenres);
            }

            return new Response<NovelResponse>(novelMapping, null);
        }

        private async Task<IReadOnlyList<string>> GetGenreFromNovel(List<NovelGenre> novelsgenres)
        {
            List<string> genres = new List<string>();
            foreach (var ng in novelsgenres)
            {
                var genre = await _context.Genres.FindAsync(ng.GenreID);
                if (genre != null)
                {
                    genres.Add(genre.Name);
                }
            }
            return genres;
        }

        public async Task<Response<RequestParameter<NovelResponse>>> GetNovels(int pageIndex, int pageSize, int? genreID, Guid? userID)
        {
            var novels = await _context.Novels.Where(n => n.DeletedDate == null).OrderBy(n => n.NovelID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await _context.Novels.CountAsync();

            if(genreID != null || userID != null)
            {
                novels = FilterNovels(novels, genreID, userID);
                totalCount = await CountData(genreID, userID);
            }

            var noveslMapping = novels.Select(n => _mapper.Map<NovelResponse>(n)).ToList();

            foreach (var novel in noveslMapping)
            {
                var userResult = novel.PublisherID != null ? await _userService.GetUserByIDORCode(novel.PublisherID.Value, null) : null;
                if (userResult != null)
                {
                    novel.User = userResult.Data;
                }

                var chaptersResult = await _chapterService.GetAllChapters(1, 5, novel.NovelID);
                if (chaptersResult != null && chaptersResult.Data != null && chaptersResult.Data.Items != null && chaptersResult.Data.Items.Any())
                {
                    novel.Chapter = chaptersResult.Data.Items;
                }
            }

            return new Response<RequestParameter<NovelResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<NovelResponse>
                {
                    TotalItemsCount = totalCount,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Items = novels.Select(n => _mapper.Map<NovelResponse>(n)).ToList()
                }
            };
        }

        public async Task<Response<string>> UpdateNovel(int novelID, IFormFile? file, UpdateNovelViewModel request)
        {
            if (novelID != request.NovelID)
            {
                throw new ApiException("Truyện không hợp lệ");
            }
            var novel = await _context.Novels.FirstOrDefaultAsync(n => n.DeletedDate == null && n.NovelID == novelID);
            if (novel == null)
            {
                throw new ApiException("Truyện không tồn tại hoặc đã bị xóa.");
            }
            _mapper.Map(request, novel);

            if (file != null)
            {
                var imageResult = await _imageService.UploadImage(file);

                if (imageResult.Succeeded && imageResult.Data != null)
                {
                    novel.CoverImage = imageResult.Data;
                }
            }

            _context.Novels.Update(novel);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }

        public async Task<Response<string>> UpdateCount(int novelID, string type)
        {
            var novel = await _context.Novels.FindAsync(novelID);
            if (novel == null)
            {
                throw new ApiException("Truyện không tồn tại.");
            }

            switch (type)
            {
                case "view":
                    novel.ViewCount += 1;
                    break;
                case "favorite":
                    novel.FavoriteCount += 1;
                    break;
            }

            _context.Novels.Update(novel);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }

        public async Task<Response<string>> PermanentlyDelete(int novelID)
        {
            var novel = await _context.Novels.FindAsync(novelID);
            if (novel == null)
            {
                throw new ApiException("Truyện này không tồn tại");
            }
            _context.Novels.Remove(novel);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa truyện thành công.", null);
        }

        private List<Novel> FilterNovels(List<Novel> data, int? genreID, Guid? userID)
        {
            List<Novel> novels = new List<Novel>();
            if (genreID != null && genreID != -1)
            {
                novels = data.
                    Join(
                        _context.NovelGenres,
                        n => n.NovelID,
                        ng => ng.NovelID,
                        (n, ng) => new { Novel = n, NovelGenre = ng }
                    ).
                    Join(
                        _context.Genres,
                        ng => ng.NovelGenre.GenreID,
                        g => g.GenreID,
                        (ng, g) => new { Novel = ng.Novel, Genre = g }
                    ).Where(ng => ng.Genre.GenreID == genreID).Select(ng => ng.Novel).ToList();
            }
            if(userID != null)
            {
                novels = data.Where(n => n.PublisherID == userID).ToList();
            }
            return novels;
        }

        private async Task<int> CountData(int? genreID, Guid? userID)
        {
            int totalCount = 0;
            if (genreID != null && genreID != -1)
            {
                totalCount = await _context.Novels
                .Join(
                    _context.NovelGenres,
                    n => n.NovelID,
                    ng => ng.NovelID,
                    (n, ng) => new { Novel = n, NovelGenre = ng }
                )
                .Join(
                   _context.Genres,
                    ng => ng.NovelGenre.GenreID,
                    g => g.GenreID,
                    (ng, g) => new { Novel = ng.Novel, Genre = g }
                )
                .Where(ng => ng.Genre.GenreID == genreID)
                .CountAsync();
            }
            if(userID != null)
            {
                totalCount = await _context.Novels.Where(n => n.PublisherID == userID).CountAsync();
            }
            return totalCount;
        }
    }
}
