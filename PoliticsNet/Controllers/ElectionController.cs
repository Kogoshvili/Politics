using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using PoliticsNet.DTO;

namespace PoliticsNet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ElectionController : ControllerBase
    {
        private readonly IElectionRespository _election;
        private readonly IUserRespository _user;
        public ElectionController(IUserRespository user, IElectionRespository election)
        {
            _user = user;
            _election = election;

        }
        [AllowAnonymous]
        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            return Ok(await _election.GetCandidates());
        }

        [HttpPost("voter")]
        public async Task<IActionResult> VoterExists(IncomingVoter voter)
        {
            return Ok(await _election.VoterExists(voter.UserName, voter.UserId));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Vote(int id, IncomingVote vote)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            List<string> ranked = new List<string> { vote.First, vote.Second, vote.Third, vote.Fourth, vote.Fifth };
            ranked = ranked.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            if (ranked.Count == ranked.Distinct().Count())
            {
                var user = await _user.GetUser(id);
                if(await _election.VoterExists(user.UserName, user.Id)){
                    return BadRequest("You already voted");
                }
                _election.AddVote(vote.Prime, vote.First, vote.Second, vote.Third, vote.Fourth, vote.Fifth);
                _election.CreateVoter(user.UserName, user.Id);
                if(await _election.SaveVote()){
                    return NoContent();
                }
            }

            return BadRequest("Something went wrong");
        }
    }
}