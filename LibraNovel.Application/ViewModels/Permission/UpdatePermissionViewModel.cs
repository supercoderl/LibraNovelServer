using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Permission
{
    public class UpdatePermissionViewModel
    {
        public int PermissionID { get; set; }
        public int PermissionCode { get; set; }
        public string Title { get; set; }
        public string TitleEN { get; set; }
        public int? Parent { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid? UpdatedBy { get; set; }
    }
}
