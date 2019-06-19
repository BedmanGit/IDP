using System.Collections.Generic;
using System.IO;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        //public void SeedUsers()
        //{
        //    var userData = File.ReadAllText("Data/UserSeedData.json");
        //    var users = JsonConvert.DeserializeObject<List<User>>(userData);
        //    foreach (User u in users)
        //    {
        //        byte[] passwordhash, passwordsalt;
        //        CreatePasswordHassPasswordSalt("password", out passwordhash, out passwordsalt);
        //        u.PasswordHash = passwordhash;
        //        u.PasswordSalt = passwordsalt;
        //        u.UserName = u.UserName.ToLower();
        //        _context.Users.Add(u);
        //    }
        //    _context.SaveChanges();
        //}
        
        //private void CreatePasswordHassPasswordSalt(string password, out byte[] passwordhash, out byte[] passwordsalt)
        //{
        //    using(var hmac = new System.Security.Cryptography.HMACSHA512())
        //    {
        //        passwordsalt = hmac.Key;
        //        passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //    }
        //}
    }
}