using System.Collections.Generic;

namespace PoliticsNet.DTO
{
    public class RatingLikesToReturn
    {
        public List<int> UserId { get; set; }
        public int Sum { get; set; }
    }
}