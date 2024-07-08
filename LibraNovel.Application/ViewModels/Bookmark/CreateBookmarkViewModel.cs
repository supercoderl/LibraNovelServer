using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Bookmark
{
    public class CreateBookmarkViewModel
    {
        public Guid? UserID { get; set; }
        public int? NovelID { get; set; }
        public int? ChapterID { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
