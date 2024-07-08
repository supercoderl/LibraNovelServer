using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Comment;
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
    public class CommentService : ICommentService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CommentService(LibraNovelContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Response<CommentResponse>> CreateComment(CreateCommentViewModel request)
        {
            var comment = _mapper.Map<Comment>(request);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return new Response<CommentResponse>(_mapper.Map<CommentResponse>(comment), null);
        }

        public async Task<Response<string>> DeleteComment(int commentID)
        {
            var comment = await _context.Comments.FindAsync(commentID);
            if (comment == null)
            {
                throw new ApiException("Bình luận không tồn tại");
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa bình luận thành công", null);
        }

        public async Task<Response<RequestParameter<CommentResponse>>> GetAllComments(int pageIndex, int pageSize, Guid? userID, int? novelID, int? chapterID)
        {
            var comments = await _context.Comments.OrderByDescending(c => c.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            if (userID != null)
            {
                comments = comments.Where(c => c.UserID == userID).ToList();
            }
            if (novelID != null)
            {
                comments = comments.Where(c => c.NovelID == novelID).ToList();
            }
            if (chapterID != null)
            {
                comments = comments.Where(c => c.ChapterID == chapterID).ToList();
            }

            var commentsDTO = comments.Select(c => _mapper.Map<CommentResponse>(c)).ToList();
            if (commentsDTO.Any())
            {
                foreach (var commentDTO in commentsDTO)
                {
                    if (commentDTO.UserID != null)
                    {
                        var user = await _context.Users.FindAsync(commentDTO.UserID);
                        if (user != null)
                        {
                            commentDTO.Name = string.Concat(user.LastName, " ", user.FirstName);
                            commentDTO.Avatar = user.Avatar;
                        };
                    }
                    if (commentDTO.NovelID != null)
                    {
                        var novel = await _context.Novels.FindAsync(commentDTO.NovelID);
                        if (novel != null)
                        {
                            commentDTO.Novel = novel.Title;
                        };
                    }
                    if (commentDTO.ChapterID != null)
                    {
                        var chapter = await _context.Chapters.FindAsync(commentDTO.ChapterID);
                        if (chapter != null)
                        {
                            commentDTO.Chapter = chapter.Title;
                        };
                    }
                }
            }

            return new Response<RequestParameter<CommentResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<CommentResponse>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = comments.Count,
                    Items = commentsDTO
                }
            };
        }

        public async Task<Response<string>> UpdateComment(int commentID, UpdateCommentViewModel request)
        {
            if (commentID != request.CommentID)
            {
                throw new ApiException("Bình luận không hợp lệ");
            }
            var comment = await _context.Comments.FindAsync(commentID);
            if (comment == null)
            {
                throw new ApiException("Bình luận không tồn tại");
            }
            _mapper.Map(request, comment);
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật bình luận thành công", null);
        }
    }
}
