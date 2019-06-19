using IDP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.Services
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        User GetUserById(string Id);
        User GetUserByEmail(string email);
        User GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<UserLogin> GetUserLoginsById(string Id);
        IEnumerable<UserClaim> GetUserClaimsById(string Id);
        bool AreUserCredentialsValid(string username, string password);
        bool IsUserActive(string Id);
        void AddUser(User user);
        void AddUserLogin(string Id, string loginProvider, string providerKey);
        void AddUserClaim(string Id, string claimType, string claimValue);
        bool Save();
    }
}
