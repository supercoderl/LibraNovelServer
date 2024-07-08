using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Comment;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface ICommentService
    {
        Task<Response<RequestParameter<CommentResponse>>> GetAllComments(int pageIndex,  int pageSize, Guid? userID, int? novelID, int? chapterID);
        Task<Response<CommentResponse>> CreateComment(CreateCommentViewModel request);
        Task<Response<string>> UpdateComment(int commentID, UpdateCommentViewModel request);
        Task<Response<string>> DeleteComment(int commentID);
    }
}
