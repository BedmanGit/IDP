using IDP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.Services
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserByUsername(string username);
        User GetUserById(int Id);
        User GetUserByEmail(string email);
        User GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<UserLogin> GetUserLoginsById(int Id);
        IEnumerable<UserClaim> GetUserClaimsById(int Id);
        bool AreUserCredentialsValid(string username, string password);
        bool IsUserActive(int Id);
        void AddUser(User user);
        void AddUserLogin(int Id, string loginProvider, string providerKey);
        void AddUserClaim(int Id, string claimType, string claimValue);
        bool Save();
    }
}
