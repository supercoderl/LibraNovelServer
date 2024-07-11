using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.User
{
    public class UserInformation
    {
        public Guid UserID { get; set; }
        public string? Avatar {  get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserCode { get; set; }
        public string Gender { get; set; }
        public int IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }

        public List<string>? Roles { get; set; }
    }
}
