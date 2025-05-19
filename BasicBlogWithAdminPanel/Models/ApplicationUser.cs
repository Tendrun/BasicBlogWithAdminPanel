using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models
{
    public class ApplicationUser : IdentityUser {
        public UserRole Role { get; set; } = UserRole.User;
    }
}
