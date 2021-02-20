using System.Collections.Generic;
using System.Threading.Tasks;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IElectionRespository
    {
        Task<IEnumerable<Candidate>> GetCandidates();
        void AddVote(string Prime, string First, string Second, string Third, string Fourth, string Fifth);
        void CreateVoter(string Username, int UserId);
        Task<bool> VoterExists(string Username, int UserId);
        Task<bool> SaveVote();
    }
}