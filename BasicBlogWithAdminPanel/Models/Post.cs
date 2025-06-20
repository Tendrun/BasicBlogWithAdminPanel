﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = default!;

        [Required]
        public string Content { get; set; } = default!;

        public string Author { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        /* NEW — one-to-many */
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
