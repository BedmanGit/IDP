using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public readonly IConfiguration _config ;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository authRepo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _authRepo = authRepo;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO user)
        {
            user.UserName = user.UserName.ToLower();
            if (await _authRepo.UserExists(user.UserName))
                return BadRequest("User already exists.");

            User _user = new User()
            {
                UserName = user.UserName
            };

            var createdUser = await _authRepo.Register(_user, user.Password);

            return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            User _user = await _authRepo.Login(user.UserName.ToLower(), user.Password);

            if (_user == null)
                return Unauthorized();

            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier, _user.ID.ToString()),
                new Claim(ClaimTypes.Name, _user.UserName)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").ToString()));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddHours(1.0),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserForListDTO userForList = _mapper.Map<UserForListDTO>(_user);
            return Ok(new 
            {
                token = tokenHandler.WriteToken(token),
                user = userForList
            });
        }
        
    }
}