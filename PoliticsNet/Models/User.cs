using System;
using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class User
    {
        public int Id {get;set;}
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string GeoId { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<RatingLike> RatingLikes { get; set; }
        public virtual ICollection<RatingCommentLike> RatingCommentLikes { get; set; }
        public virtual ICollection<ActivityLike> ActivityLike { get; set; }
        public virtual ICollection<ActivityCommentLike> ACtivityCommentLike { get; set; }

        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User(){
            Role = "Member";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}