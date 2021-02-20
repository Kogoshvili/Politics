using PoliticsNet.Models;

namespace PoliticsNet.DTO
{
    public class ProviderToReturn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ProviderType Type { get; set; }
        public string Image { get; set; }
    }
}