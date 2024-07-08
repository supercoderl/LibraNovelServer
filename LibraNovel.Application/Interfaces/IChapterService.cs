using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Chapter;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IChapterService
    {
        Task<Response<RequestParameter<ChapterResponse>>> GetAllChapters(int  pageNumber, int pageSize, int? novelID);
        Task<Response<ChapterResponse>> GetChapterByID(int chapterID);
        Task<Response<string>> CreateChapter(CreateChapterViewModel request);
        Task<Response<string>> UpdateChapter(int chapterID, UpdateChapterModel request);
        Task<Response<string>> DeleteChapter(int chapterID);
    }
}
