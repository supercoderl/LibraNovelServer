using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Genre
{
    public class GenreResponse
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
    }
}
