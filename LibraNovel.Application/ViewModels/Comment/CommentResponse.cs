using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Comment
{
    public class CommentResponse
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public Guid? UserID { get; set; }
        public int? NovelID { get; set; }
        public int? ChapterID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public string? Novel { get; set; }
        public string? Chapter { get; set; }
    }
}
