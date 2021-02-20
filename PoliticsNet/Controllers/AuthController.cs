using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using PoliticsNet.Models;
using PoliticsNet.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace PoliticsNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IPhoneRespository _phone;
        public AuthController(IAuthRepository repo,IConfiguration config, IPhoneRespository phone)
        {
            _phone = phone;
            _repo = repo;
            _config = config;
        }

        [HttpPost("phone")]
        public async Task<IActionResult> PhoneVerification(IncomingPhone phone)
        {
            if (await _repo.PhoneExists(phone.Phone))
                return BadRequest("Phone Exists");

            var status = await _phone.SendCode(phone.Phone);
            return Ok(status == "pending");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister user)
        {
            //VALIDATE REQUES!
            if (await _repo.UserNameExists(user.Username))
                return BadRequest("Username Exists");

            if (await _repo.PhoneExists(user.Phone))
                return BadRequest("Phone Exists");

            var result = await _phone.CheckCode(user.Phone, user.Code);

            if (result != "approved")
            {
                return BadRequest("Wrong Code");
            }

            var userToCreate = new User
            {
                UserName = user.Username,
                Phone = user.Phone
            };

            var createdUser = await _repo.Register(userToCreate, user.Password);

            var token = CreateToken(createdUser);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin user)
        {
            var userForRepo = await _repo.Login(user.Username, user.Password);

            if (userForRepo == null)
                return Unauthorized();

            var token = CreateToken(userForRepo);

            return Ok(new { token });
        }

        private string CreateToken(User user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(14),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}