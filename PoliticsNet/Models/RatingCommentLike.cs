namespace PoliticsNet.Models
{
    public class RatingCommentLike
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual RatingComment Comment { get; set; }
    }
}