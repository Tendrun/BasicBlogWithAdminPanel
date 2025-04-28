using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "User"; // Domyślna rola
    }
}
