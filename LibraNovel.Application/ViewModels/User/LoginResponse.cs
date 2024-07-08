using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.User
{
    public class LoginResponse
    {
        public TokenResponse AccessToken {  get; set; }
        public TokenResponse RefreshToken { get; set; }
        public UserResponse User { get; set; }
    }
    public class UserResponse
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public List<int>? Roles { get; set; }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
        public int ExpireMinutes { get; set; }
        public long? Exp {  get; set; }
    }
}
