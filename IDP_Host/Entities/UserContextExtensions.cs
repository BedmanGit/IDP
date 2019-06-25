using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.Entities
{
    public static class UserContextExtensions
    {
        public static void EnsureSeedDataForContext(this UserContext context)
        {
            // Add 2 demo users if there aren't any users yet
            if (context.Users.Any())
            {
                return;
            }

            // init users
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    UserName = "eva",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                         new UserClaim("role", "admin"),
                         new UserClaim("given_name", "eva"),
                         new UserClaim("family_name", "Doe"),
                         new UserClaim("dob", "01/01/1989"),
                         new UserClaim("address", "Main Road 1"),
                         new UserClaim("city", "NYC"),
                         new UserClaim("country", "USA"),
                         new UserClaim("gender", "female"),
                         new UserClaim("interests", "watch movies on the street"),
                         new UserClaim("introduction", "hi there, wutup"),
                         new UserClaim("knownas", "Eva the kid"),
                         new UserClaim("lookingfor", "someone who can watch movies with")
                    }
                },
                new User()
                {
                    UserId = 2,
                    UserName = "Claire",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                         new UserClaim("role", "admin"),
                         new UserClaim("given_name", "Claire"),
                         new UserClaim("family_name", "Doe"),
                         new UserClaim("dob", "04/01/1985"),
                         new UserClaim("address", "Main Road 2"),
                         new UserClaim("city", "Edison"),
                         new UserClaim("country", "NJ"),
                         new UserClaim("gender", "female"),
                         new UserClaim("interests", "pingpong"),
                         new UserClaim("introduction", "Hi there, this is Claire!"),
                         new UserClaim("knownas", "Super-Girl"),
                         new UserClaim("lookingfor", "someone who can fly")
                }
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
