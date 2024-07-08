using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Role
{
    public class CreateMappingRoleWithPermissionsRequest
    {
        public int RoleID { get; set; }
        public List<string> Permissions { get; set; }
    }
}
