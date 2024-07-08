using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Permission
{
    public class CreatePermissionViewModel
    {
        public int PermissionCode { get; set; }
        public string Title { get; set; }
        public string TitleEN { get; set; }
        public int? Parent { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? CreatedBy { get; set; }
    }
}
