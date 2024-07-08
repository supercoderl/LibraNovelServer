using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraNovel.Application.ViewModels.Bookmark;

namespace LibraNovel.Application.ViewModels.Novel
{
    public class CreateNovelViewModel
    {
        public string Title { get; set; }
        public string? Description { get; set; } = "";
        public string? OtherName { get; set; }
        public int TotalPages { get; set; }
        public string? CoverImage { get; set; } = "";
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public Guid? PublisherID { get; set; }
        public int Status { get; set; } = (int)NovelEnum.Creating;
    }
}
