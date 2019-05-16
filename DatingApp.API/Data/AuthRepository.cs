using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            User user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return null;
            
            if(!VerifyPasswordHash(user, password))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(User user, string password)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
            {
                var passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i =0; i < passwordhash.Length; i++)
                {
                    if (passwordhash[i] != user.PasswordHash[i])
                        return false;                       
               }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHassPasswordSalt(password, out passwordHash, out passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHassPasswordSalt(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(a => a.UserName == username))
            return true;
            return false;
        }
    }
}