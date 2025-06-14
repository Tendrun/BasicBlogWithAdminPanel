using System;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models
{
    /// <summary>
    /// Blog-post aggregate root.
    /// </summary>
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = default!;

        [Required]
        public string Content { get; set; } = default!;

        /// <summary>
        /// UserName of the author (filled from <c>User.Identity.Name</c>).
        /// </summary>
        public string Author { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Marks the row as deleted without physically removing it (soft delete).
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
