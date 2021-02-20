namespace PoliticsNet.Models
{
    public class ActivityCommentLike
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ActivityComment Comment { get; set; }
    }
}