using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsersAsync();
            var usersReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);

            return Ok(usersReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
        
            var user = await _repo.GetUserAsync(id);
            var userReturn = _mapper.Map<UserForDetailDTO>(user);
            return Ok(userReturn);
        }
        [HttpGet]
        [Route("GetUserListDTO/{id}")]
        public async Task<IActionResult> GetUserListDTO(int id)
        {
            var user = await _repo.GetUserAsync(id);
            var userReturn = _mapper.Map<UserForListDTO>(user);
            return Ok(userReturn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdate)
        {
            if (id != int.Parse(User.Claims.FirstOrDefault(a => a.Type == "sub").Value))
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUserAsync(id);
            Type t = userForUpdate.GetType();
            foreach (var p in t.GetProperties())
            {
                if(userFromRepo.Claims.Any(c => c.ClaimType == p.Name))
                {
                    userFromRepo.Claims.Where(c => c.ClaimType == p.Name).FirstOrDefault().ClaimValue = p.GetValue(userForUpdate).ToString();
                }
                else
                {
                    userFromRepo.Claims.Add(new UserClaim()
                    {
                        UserId = id,
                        ClaimType = p.Name,
                        ClaimValue = p.GetValue(userForUpdate).ToString()

                    });

                }
            }

            if (await _repo.SaveAllAsync())
            return NoContent();

            throw new Exception($"Updating user {id} failed on save.");

        }
    }
}