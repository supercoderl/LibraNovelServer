using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Chapter
{
    public class CreateChapterViewModel
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public int ChapterNumber { get; set; }
        public DateTime? PublishDate { get; set; } = DateTime.Now;
        public int? NovelID { get; set; }
    }
}
