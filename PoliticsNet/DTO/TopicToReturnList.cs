namespace PoliticsNet.DTO
{
    public class TopicToReturnList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Comments { get; set; }
        public virtual RatingLikesToReturn Likes { get; set; }
    }
}