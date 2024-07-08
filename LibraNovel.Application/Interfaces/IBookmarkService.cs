using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Bookmark;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IBookmarkService
    {
        Task<Response<RequestParameter<BookmarkResponse>>> GetAllBookmarks(int pageIndex,  int pageSize);
        Task<Response<string>> CreateBookmark(CreateBookmarkViewModel request);
        Task<Response<string>> DeleteBookmark(int bookmarkID);
    }
}
