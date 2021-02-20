namespace PoliticsNet.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}