using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Rating
{
    public class CreateRatingViewModel
    {
        public int RatingID { get; set; }
        public int Score { get; set; }
        public Guid UserID { get; set; }
        public int NovelID { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
