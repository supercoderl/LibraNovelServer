using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Permission
{
    public class PermissionResponse
    {
        public int PermissionID { get; set; }
        public int PermissionCode { get; set; }
        public string Title { get; set; }
        public string TitleEN { get; set; }
        public int? Parent {  get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public string? ParentTitle { get; set; }
        public List<PermissionResponse> Children { get; set; } = new List<PermissionResponse>();
    }
}
