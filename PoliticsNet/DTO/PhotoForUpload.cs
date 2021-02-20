using Microsoft.AspNetCore.Http;

namespace PoliticsNet.DTO
{
    public class PhotoForUpload
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
    }
}