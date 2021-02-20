using System;
using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class RatingTopic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Boolean Visible { get; set; }
        public virtual ICollection<RatingComment> RatingComment { get; set; }
        public virtual ICollection<RatingLike> RatingLikes { get; set; }

        public RatingTopic(){
            RatingComment = new List<RatingComment>();
            RatingLikes = new List<RatingLike>();
        }
    }
}