using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicBlogWithAdminPanel.Models
{
    public class Comment
    {
        public int Id { get; set; }

        /* FK must match Post.Id type (int) */
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }

        [Required]
        public string Content { get; set; } = default!;

        public string Author { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /* navigation */
        public Post? Post { get; set; }
    }
}
