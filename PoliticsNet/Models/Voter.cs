using System;

namespace PoliticsNet.Models
{
    public class Voter
    {
        public int Id { get; set; }
        public string HashId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Voter(){
            CreatedAt = DateTime.Now;
        }
    }
}