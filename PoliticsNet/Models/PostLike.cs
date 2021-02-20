namespace PoliticsNet.Models
{
    public class PostLike
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}