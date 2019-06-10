using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";
            await WriteOutIdentityInformation();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"Identity Token: {identityToken}");

            foreach(var claim in User.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value: {claim.Value}");
            }

        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("MyCookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [Authorize]
        public async Task Login()
        {
    
        }
    }
}
