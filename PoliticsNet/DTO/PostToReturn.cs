using System;
using System.Collections.Generic;

namespace PoliticsNet.DTO
{
    public class PostToReturn
    {
        public int Id { get; set; }
        public List<ImageToReturn> Images { get; set; }
        public string Content { get; set; }
        public CategoryToReturn Category { get; set; }
        public UserLite User { get; set; }
        public ProviderForPost Provider { get; set; }
        public List<int> Likes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}