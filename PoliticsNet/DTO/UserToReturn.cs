using System.Collections.Generic;

namespace PoliticsNet.DTO
{
    public class UserToReturn
    {
        public int Id {get;set;}
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string GeoId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<int> Posts { get; set; }
        public string Avatar { get; set; }
    }
}