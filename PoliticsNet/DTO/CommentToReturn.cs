using System.Collections.Generic;

namespace PoliticsNet.DTO
{
    public class CommentToReturn
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Side { get; set; }
        public virtual List<int> Likes { get; set; }
        public virtual UserLite User { get; set; }
    }
}