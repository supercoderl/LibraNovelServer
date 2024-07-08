using LibraNovel.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Rating
{
    public class RatingResponse
    {
        public int RatingID { get; set; }
        public int Score { get; set; }
        public Guid? UserID { get; set; }
        public string? Author { get; set; }
        public int? NovelID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
