using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Rating;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public RatingService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<string>> CreateRating(CreateRatingViewModel request)
        {
            var rating = _mapper.Map<Rating>(request);
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo đánh giá thành công", null);
        }

        public async Task<Response<string>> DeleteRating(int ratingsID)
        {
            var rating = await _context.Ratings.FindAsync(ratingsID);
            if(rating == null)
            {
                throw new ApiException("Đánh giá không tồn tại");
            }
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa đánh giá thành công", null);
        }

        public async Task<Response<RequestParameter<RatingResponse>>> GetAllRatings(int pageIndex, int pageSize, int? novelID)
        {
            var ratings = await _context.Ratings.Where(r => r.NovelID == novelID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Response<RequestParameter<RatingResponse>>
            {
                Data =
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = ratings.Count,
                    Items = ratings.Select(r => _mapper.Map<RatingResponse>(r)).ToList()
                }
            };
        }
    }
}
