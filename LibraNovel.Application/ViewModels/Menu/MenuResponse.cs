﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Menu
{
    public class MenuResponse
    {
        public int MenuID { get; set; }
        public string Title { get; set; }
        public string? Icon { get; set; }
        public string? URL { get; set; }
        public string? Path { get; set; }
        public int? ParentID { get; set; }
        public int? OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}