using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDP.Services
{
    public class UserRepository : IUserRepository
    {
        UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        private string GetLanId(string windowsId)
        {
            return windowsId.ToUpper().Replace(@"\", "").Replace("WINDOWS", "");
        }
        public bool AreUserCredentialsValid(string username, string password)
        {
            // get the user
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            return (user.Password == password && !string.IsNullOrWhiteSpace(password));
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Claims.Any(c => c.ClaimType == "email" && c.ClaimValue == email));
        }

        public User GetUserByProvider(string loginProvider, string providerKey)
        {
            var u = _context.Users.Include(c => c.Claims).Include(l => l.Logins);
            return u.FirstOrDefault(a => 
                    a.Logins.Any(l => l.LoginProvider.ToUpper() == loginProvider.ToUpper() && GetLanId(l.ProviderKey) == GetLanId(providerKey)));
    
        }

        public User GetUserById(int Id)
        {
            return _context.Users.Include(c => c.Claims).FirstOrDefault(u => u.UserId == Id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.Include(c => c.Claims).FirstOrDefault(u => GetLanId(u.UserName) == GetLanId(username));
        }

        public IEnumerable<UserClaim> GetUserClaimsById(int Id)
        {
            // get user with claims
            var user = _context.Users.Include("Claims").FirstOrDefault(u => u.UserId == Id);
            if (user == null)
            {
                return new List<UserClaim>();
            }
            return user.Claims.ToList();
        }

        public IEnumerable<UserLogin> GetUserLoginsById(int Id)
        {
            var user = _context.Users.Include("Logins").FirstOrDefault(u => u.UserId == Id);
            if (user == null)
            {
                return new List<UserLogin>();
            }
            return user.Logins.ToList();
        }

        public bool IsUserActive(int Id)
        {
            var user = GetUserById(Id);
            return user.IsActive;
         }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void AddUserLogin(int Id, string loginProvider, string providerKey)
        {
            var user = GetUserById(Id);
            if (user == null)
            {
                throw new ArgumentException("User with given Id not found.", Id.ToString());
            }

            user.Logins.Add(new UserLogin()
            {
                UserId = Id,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            });
        }

        public void AddUserClaim(int Id, string claimType, string claimValue)
        {          
            var user = GetUserById(Id);
            if (user == null)
            {
                throw new ArgumentException("User with given Id not found.", Id.ToString());
            }

            user.Claims.Add(new UserClaim(claimType, claimValue));         
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
