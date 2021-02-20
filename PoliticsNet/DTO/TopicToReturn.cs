using System.Collections.Generic;

namespace PoliticsNet.DTO
{
    public class TopicToReturn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<CommentToReturn> Comments { get; set; }
        public virtual RatingLikesToReturn RatingLikes { get; set; }
    }
}