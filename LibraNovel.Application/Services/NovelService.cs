using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Chapter;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Ocsp;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace LibraNovel.Application.Services
{
    public class NovelService : INovelService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IChapterService _chapterService;
        private readonly IImageService _imageService;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;

        public NovelService(LibraNovelContext context, IMapper mapper, IUserService userService, IChapterService chapterService, IImageService imageService, IDistributedCache cache, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _chapterService = chapterService;
            _imageService = imageService;
            _cache = cache;
            _cacheService = cacheService;
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

            await _cacheService.RemoveCacheKeysContaining("read_novels");

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

            await _cacheService.RemoveCacheKeysContaining("read_novels");

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

        public async Task<Response<RequestParameter<NovelResponse>>> GetNovels(int pageIndex, int pageSize, int? genreID, Guid? userID, string? searchText)
        {
            List<int> novelIDs = new List<int>();
            List<Guid> publisherIDs = new List<Guid>();
            List<NovelResponse>? novelResponses = new List<NovelResponse>();

            var stopwatch = Stopwatch.StartNew();

            var cacheKey = $"read_novels_{pageIndex}_{pageSize}_{genreID}_{userID}_{searchText}";
            string? cachedData = await _cacheService.GetDataFromCache<List<NovelResponse>>(cacheKey);

            List<Novel>? novels = null;

            // Attempt to get novels from cache
            if (!string.IsNullOrEmpty(cachedData))
            {
                novelResponses = JsonSerializer.Deserialize<List<NovelResponse>>(cachedData);
            }
            else
            {
                var query = _context.Novels.AsNoTracking().Where(n => n.DeletedDate == null);

                query = FilterNovels(query, genreID, userID, searchText);

                novels = await query.OrderBy(n => n.NovelID)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .Include(n => n.Chapters)
                                    .Include(n => n.Publisher)
                                    .ToListAsync();

                if (novels != null)
                {
                    novelResponses = novels.Select(novel =>
                    {
                        var novelResponse = _mapper.Map<NovelResponse>(novel);

                        if(novel.Chapters.Any())
                        {
                            novelResponse.Chapter = novel.Chapters.Select(n => new ViewModels.Novel.Chapter
                            {
                                ChapterID = n.ChapterID,
                                ChapterNumber = n.ChapterNumber,
                            }).ToList();
                        }

                        if (novel.Publisher != null)
                        {
                            novelResponse.User = new Author
                            {
                                FirstName = novel.Publisher?.FirstName,
                                LastName = novel.Publisher?.LastName
                            };
                        }

                        return novelResponse;
                    }).ToList();
                }

                await _cacheService.StoreDataToCache(cacheKey, novelResponses);
            }

            int totalCount = await CountData(genreID, userID);

            stopwatch.Stop();
            var executionTime = stopwatch.Elapsed;

            return new Response<RequestParameter<NovelResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<NovelResponse>
                {
                    TotalItemsCount = totalCount,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Items = novelResponses
                },
                Message = $"GetNovels method executed in {executionTime.TotalMilliseconds} ms"
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

            await _cacheService.RemoveCacheKeysContaining("read_novels");

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

        private IQueryable<Novel> FilterNovels(IQueryable<Novel> query, int? genreID, Guid? userID, string? searchText)
        {
            if (genreID.HasValue && genreID != -1)
            {
                query = query.
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
                    ).Where(ng => ng.Genre.GenreID == genreID).Select(ng => ng.Novel);
            }

            if (userID.HasValue)
            {
                query = query.Where(n => n.PublisherID == userID);
            }

            if(!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(n => n.Title.ToLower().Contains(searchText.ToLower()));
            }

            return query;
        }

        private async Task<int> CountData(int? genreID, Guid? userID)
        {
            int totalCount = await _context.Novels.CountAsync();
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
            if (userID != null)
            {
                totalCount = await _context.Novels.Where(n => n.PublisherID == userID).CountAsync();
            }
            return totalCount;
        }
    }
}
