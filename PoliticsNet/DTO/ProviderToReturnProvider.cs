using System;
using System.Collections.Generic;
using PoliticsNet.Models;

namespace PoliticsNet.DTO
{
    public class ProviderToReturnProvider
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ProviderType Type { get; set; }
        public string Description { get; set; }
        public virtual ICollection<int> Members { get; set; }
        public virtual ICollection<int> Posts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}