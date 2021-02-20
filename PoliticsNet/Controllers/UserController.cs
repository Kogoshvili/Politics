using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using PoliticsNet.DTO;

namespace PoliticsNet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRespository _repo;
        private readonly IMapper _mapper;
        public UserController(IUserRespository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserToReturn>(user);
            return Ok(userToReturn);
        }
    }
}