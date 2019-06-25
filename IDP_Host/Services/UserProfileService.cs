using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Extensions;
using System.Security.Claims;

namespace IDP.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserRepository _UserRepository;

        public UserProfileService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _UserRepository.GetUserClaimsById(Int32.Parse(subjectId));

            context.IssuedClaims = claimsForUser.Select
              (c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = _UserRepository.IsUserActive(Int32.Parse(subjectId));

            return Task.FromResult(0);
        }
    }
}
