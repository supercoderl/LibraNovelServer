using System.ComponentModel.DataAnnotations;

namespace LibraNovel.Application.ViewModels.User
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is required")]
        public string PasswordHash { get; set; }

        public string? Avatar {  get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserCode { get; set; }
        public string? Gender { get; set; }
        public string? Provider { get; set; }
        public bool? IsActive { get; set; } = true;
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
    }
}
