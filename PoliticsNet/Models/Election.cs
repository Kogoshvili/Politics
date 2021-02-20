namespace PoliticsNet.Models
{
    public class Election
    {
        public int Id { get; set; }
        public virtual Candidate Candidate { get; set; }
        public int VoteP { get; set; }
        public int Vote1 { get; set; }
        public int Vote2 { get; set; }
        public int Vote3 { get; set; }
        public int Vote4 { get; set; }
        public int Vote5 { get; set; }
    }
}