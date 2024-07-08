using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Bookmark;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public BookmarkService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateBookmark(CreateBookmarkViewModel request)
        {
            var bookmark = _mapper.Map<Bookmark>(request);
            await _context.Bookmarks.AddAsync(bookmark);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo đánh dấu thành công", null);
        }

        public async Task<Response<string>> DeleteBookmark(int bookmarkID)
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkID);
            if(bookmark == null)
            {
                throw new ApiException("Đánh dấu không tồn tại");
            }
            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa đánh dấu thành công", null);
        }

        public Task<Response<RequestParameter<BookmarkResponse>>> GetAllBookmarks(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
