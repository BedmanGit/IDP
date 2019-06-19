using IdentityServer4.Services;
using IDP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IDP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IUserRepository _UserRepository;
        private readonly IIdentityServerInteractionService _interaction;

        public UserRegistrationController(IUserRepository UserRepository,
             IIdentityServerInteractionService interaction)
        {
            _UserRepository = UserRepository;
            _interaction = interaction;
        }

        [HttpGet]
        public IActionResult RegisterUser(RegistrationInputModel registrationInputModel)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = registrationInputModel.ReturnUrl,
                Provider = registrationInputModel.Provider,
                ProviderUserId = registrationInputModel.ProviderUserId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // create user + claims
                var userToCreate = new Entities.User();
                userToCreate.Password = model.Password;
                userToCreate.Username = model.Username;
                userToCreate.IsActive = true;
                userToCreate.Claims.Add(new Entities.UserClaim("country", model.Country));
                userToCreate.Claims.Add(new Entities.UserClaim("address", model.Address));
                userToCreate.Claims.Add(new Entities.UserClaim("given_name", model.Firstname));
                userToCreate.Claims.Add(new Entities.UserClaim("family_name", model.Lastname));
                userToCreate.Claims.Add(new Entities.UserClaim("email", model.Email));
                userToCreate.Claims.Add(new Entities.UserClaim("role", model.Role));

                // if we're provisioning a user via external login, we must add the provider &
                // user id at the provider to this user's logins
                if (model.IsProvisioningFromExternal)
                {
                    userToCreate.Logins.Add(new Entities.UserLogin()
                    {
                        LoginProvider = model.Provider,
                        ProviderKey = model.ProviderUserId
                    });
                }

                // add it through the repository
                _UserRepository.AddUser(userToCreate);

                if (!_UserRepository.Save())
                {
                    throw new Exception($"Creating a user failed.");
                }

                if (!model.IsProvisioningFromExternal)
                {
                    // log the user in
                    // create claims  
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userToCreate.Username),
                        new Claim(ClaimTypes.Sid, userToCreate.Id)
                    };

                    // create identity  
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

                    // create principal  
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                }

                // continue with the flow     
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // ModelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }

    }
}
