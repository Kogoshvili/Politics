using System.Collections.Generic;

namespace PoliticsNet.Models
{
    public class ActivityComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Side { get; set; }
        public virtual ICollection<ActivityCommentLike> Likes { get; set; }
        public virtual User User { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
    }
}