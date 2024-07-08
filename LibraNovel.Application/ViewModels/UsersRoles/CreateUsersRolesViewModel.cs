using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.UsersRoles
{
    public class CreateUsersRolesViewModel
    {
        public List<string> Roles { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
