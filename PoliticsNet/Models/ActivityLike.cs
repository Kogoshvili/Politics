namespace PoliticsNet.Models
{
    public class ActivityLike
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public int Value { get; set; }
        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }
    }
}