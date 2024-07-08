using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordEmail
    {
        [EmailAddress]
        public string Email { get; set; }
    }

    public class VerifyResetPasswordCode
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
