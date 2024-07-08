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
    }
}
