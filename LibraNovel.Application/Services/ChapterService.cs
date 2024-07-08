using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Chapter;
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
    public class ChapterService : IChapterService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public ChapterService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateChapter(CreateChapterViewModel request)
        {
            var chapter = _mapper.Map<Chapter>(request);
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo chương thành công", null);
        }

        public async Task<Response<string>> DeleteChapter(int chapterID)
        {
            var chapter = await _context.Chapters.FindAsync(chapterID);
            if(chapter == null)
            {
                throw new ApiException("Chương không tồn tại");
            }
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa chương thành công", null);
        }

        public async Task<Response<RequestParameter<ChapterResponse>>> GetAllChapters(int pageNumber, int pageSize, int? novelID)
        {
            var chapters = await _context.Chapters.OrderBy(c => c.ChapterID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            if(novelID != null)
            {
                chapters = chapters.Where(c => c.NovelID == novelID).ToList();
            }
            
            return new Response<RequestParameter<ChapterResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<ChapterResponse>
                {
                    PageIndex = pageNumber,
                    PageSize = pageSize,
                    TotalItemsCount = chapters.Count,
                    Items = chapters.Select(c => _mapper.Map<ChapterResponse>(c)).ToList()
                }
            };
        }

        public async Task<Response<ChapterResponse>> GetChapterByID(int chapterID)
        {
            var chapter = await _context.Chapters.FindAsync(chapterID);
            if(chapter == null)
            {
                throw new ApiException("Chương không tồn tại");
            }
            return new Response<ChapterResponse>(_mapper.Map<ChapterResponse>(chapter), null);
        }

        public async Task<Response<string>> UpdateChapter(int chapterID, UpdateChapterModel request)
        {
            if(chapterID != request.ChapterID)
            {
                throw new ApiException("Chương không hợp lệ");
            }
            var chapter = await _context.Chapters.FindAsync(chapterID);
            if(chapter == null)
            {
                throw new ApiException("Chương không tồn tại");
            }
            _mapper.Map(request, chapter);  
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }
    }
}
