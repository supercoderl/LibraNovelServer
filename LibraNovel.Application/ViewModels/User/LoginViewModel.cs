using System.ComponentModel.DataAnnotations;

namespace LibraNovel.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Display(Name = "Email address")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Provider { get; set; }
    }

    public class LoginProviderViewModel
    {
        public string? Email { get; set; }
        public string? UserID { get; set; }
        public string? Avatar { get; set; }
        public string? Provider { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserCode { get; set; }
    }
}
