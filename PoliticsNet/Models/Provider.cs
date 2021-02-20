using System;
using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public virtual ProviderType Type { get; set; }
        //public virtual User Moderator { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Provider(){
             CreatedAt = DateTime.Now;
             UpdatedAt = DateTime.Now;
        }
    }
}