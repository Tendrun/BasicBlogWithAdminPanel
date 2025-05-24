using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
