using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Genre
{
    public class CreateGenreViewModel
    {
        public string Name { get; set; }
        public int? ParentID { get; set; }
    }
}
