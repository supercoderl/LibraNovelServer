using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Novel
{
    public class UpdateNovelViewModel
    {
        public int NovelID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? OtherName { get; set; }
        public int TotalPages { get; set; }
        public string? CoverImage { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid? PublisherID { get; set; }
        public int? Status { get; set; }
    }
}
