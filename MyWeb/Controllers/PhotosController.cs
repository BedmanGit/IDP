using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MyWeb.Core;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    public class PhotosController : Controller
    {
        private HttpClient _httpClient { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PhotosController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var _httpContext = await GetClient();
            var response = await _httpContext.GetAsync($"{Variables.DatingAppBaseUrl}/api/users/1/photos/getallphotos");
            if (response.IsSuccessStatusCode)
            {
                List<PhotosForListDTO> photos = await response.Content.ReadAsAsync<List<PhotosForListDTO>>();
                return View(photos);
            }
            else
                return View();
        }
        private async Task<HttpClient> GetClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", accessToken);
            }
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            return _httpClient;
        }
    }
}