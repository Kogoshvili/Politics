using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoliticsNet.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Post(){
            PostLikes = new List<PostLike>();
            Images = new List<Image>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}