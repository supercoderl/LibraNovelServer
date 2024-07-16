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

        //Create new comment
        public async Task<Response<CommentResponse>> CreateComment(CreateCommentViewModel request)
        {
            var comment = _mapper.Map<Comment>(request);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return new Response<CommentResponse>(_mapper.Map<CommentResponse>(comment), null);
        }

        //Delete comment
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

        //Get comments list
        public async Task<Response<RequestParameter<CommentResponse>>> GetAllComments(int pageIndex, int pageSize, Guid? userID, int? novelID, int? chapterID)
        {
            List<CommentResponse>? commentResponses = new List<CommentResponse>();

            var query = _context.Comments.AsNoTracking();

            query = FilterComments(query, userID, novelID);

            var comments = await query.OrderByDescending(c => c.CreatedDate)
                                      .Skip((pageIndex - 1) * pageSize)
                                      .Take(pageSize)
                                      .Include(c => c.User)
                                      .Include(c => c.Novel)
                                      .ToListAsync();

            if(comments != null)
            {
                commentResponses = comments.Select(comment =>
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);

                    if(comment.User != null)
                    {
                        commentResponse.Name = string.Concat(comment.User.LastName, " ", comment.User.FirstName);
                        commentResponse.Avatar = comment.User.Avatar;
                    }

                    if(comment.Novel != null)
                    {
                        commentResponse.Novel = comment.Novel.Title;
                    }

                    return commentResponse;
                }).ToList();    
            }

            return new Response<RequestParameter<CommentResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<CommentResponse>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = await CountData(userID, novelID),
                    Items = commentResponses
                }
            };
        }

        //Update comment
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

        //Filter
        private IQueryable<Comment> FilterComments(IQueryable<Comment> query, Guid? userID, int? novelID)
        {
            if (userID.HasValue)
            {
                query = query.Where(c => c.UserID == userID.Value);
            }

            if (novelID.HasValue)
            {
                query = query.Where(c => c.NovelID == novelID.Value);
            }

            return query;
        }

        //Count
        private async Task<int> CountData(Guid? userID, int? novelID)
        {
            int totalCount = await _context.Comments.CountAsync();
            if (userID.HasValue)
            {
                totalCount = await _context.Comments.Where(c => c.UserID == userID.Value).CountAsync();
            }
            if (novelID.HasValue)
            {
                totalCount = await _context.Comments.Where(n => n.NovelID == novelID.Value).CountAsync();
            }
            return totalCount;
        }
    }
}
