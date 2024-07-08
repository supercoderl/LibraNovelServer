using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Rating;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IRatingService
    {
        Task<Response<RequestParameter<RatingResponse>>> GetAllRatings(int pageIndex, int pageSize, int? novelID);
        Task<Response<string>> CreateRating(CreateRatingViewModel request);
        Task<Response<string>> DeleteRating(int ratingsID);
    }
}
