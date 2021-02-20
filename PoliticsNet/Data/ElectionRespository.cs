using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class ElectionRespository : IElectionRespository
    {
        private readonly DataContext _context;

        public ElectionRespository(DataContext context)
        {
            _context = context;

        }
        public string CalculateHash(string input)
{
            using (var algorithm = SHA512.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public void AddVote(string Prime, string First, string Second, string Third, string Fourth, string Fifth)
        {
            var _prime = _context.Elections.First(e => e.Candidate.Name == Prime);
            _prime.VoteP = _prime.VoteP + 1;

            if (!String.IsNullOrEmpty(First)){
                var _first = _context.Elections.First(e => e.Candidate.Name == First);
                _first.Vote1 = _first.Vote1 + 1;
            }
            if (!String.IsNullOrEmpty(Second)){
                var _second = _context.Elections.First(e => e.Candidate.Name == Second);
                _second.Vote2 = _second.Vote2 + 1;
            }
            if (!String.IsNullOrEmpty(Third)){
                var _third = _context.Elections.First(e => e.Candidate.Name == Third);
                _third.Vote3 = _third.Vote3 + 1;
            }
            if (!String.IsNullOrEmpty(Fourth)){
                var _fourth = _context.Elections.First(e => e.Candidate.Name == Fourth);
                _fourth.Vote4 = _fourth.Vote4 + 1;
            }
            if (!String.IsNullOrEmpty(Fifth)){
                var _fifth = _context.Elections.First(e => e.Candidate.Name == Fifth);
                _fifth.Vote5 = _fifth.Vote5 + 1;
            }
        }

        public void CreateVoter(string Username, int UserId)
        {
            var voter = new Voter{
                HashId = CalculateHash(Username+UserId)
            };

            _context.Voters.Add(voter);
        }

        public async Task<bool> VoterExists(string Username, int UserId)
        {
            var HashId = CalculateHash(Username+UserId);

            if(await _context.Voters.AnyAsync(v => v.HashId == HashId)){
                return true;
            }

            return false;
        }


        public async Task<bool> SaveVote()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Candidate>> GetCandidates()
        {
            return await _context.Canditates.ToListAsync();
        }
    }
}