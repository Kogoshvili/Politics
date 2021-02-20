using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PoliticsNet.DTO
{
    public class PostIncoming
    {
        public int Id { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<string> ImagesToSave { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
    }
}