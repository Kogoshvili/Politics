using System;
using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Boolean Visible { get; set; }
        public virtual ICollection<ActivityComment> ActivityComments { get; set; }
        public virtual ICollection<ActivityLike> ActivityLikes { get; set; }

        public Activity(){
            ActivityComments = new List<ActivityComment>();
            ActivityLikes = new List<ActivityLike>();
        }
    }
}