using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Comment
{
    public class UpdateCommentViewModel
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public Guid? UserID { get; set; }
        public int? NovelID { get; set; }
        public int ChapterID { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
