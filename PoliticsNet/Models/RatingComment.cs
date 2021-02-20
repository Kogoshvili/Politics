using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class RatingComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Side { get; set; }
        public virtual ICollection<RatingCommentLike> Likes { get; set; }
        public virtual User User { get; set; }
        public int TopicId { get; set; }
        public virtual RatingTopic Topic { get; set; }
    }
}