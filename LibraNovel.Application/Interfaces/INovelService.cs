using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface INovelService
    {
        Task<Response<RequestParameter<NovelResponse>>> GetNovels(int pageIndex, int pageSize, int? genreID);
        Task<Response<NovelResponse>> GetNovelByID(int novelID);
        Task<Response<string>> CreateMappingGenreWithNovel(int genreID, int novelID);   
        Task<Response<string>> CreateNovel(IFormFile? file, CreateNovelViewModel request);
        Task<Response<string>> UpdateNovel(int novelID, IFormFile? file, UpdateNovelViewModel request);
        Task<Response<string>> UpdateCount(int novelID, string type);
        Task<Response<string>> DeleteNovel(int novelID);
        Task<Response<string>> PermanentlyDelete(int novelID);
        Task<Response<string>> DropMappingGenreWithNovel(int genreID, int novelID);
    }
}
