namespace PoliticsNet.Models
{
    public class RatingLike
    {
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int Value { get; set; }
        public virtual User User { get; set; }
        public virtual RatingTopic Topic { get; set; }
    }
}