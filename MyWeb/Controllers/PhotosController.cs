using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    public class PhotosController : Controller
    {
        private HttpClient _httpClient { get; set; }
        public PhotosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("http://localhost:44319/api/users/1/photos/getphotos");
            if (response.IsSuccessStatusCode)
            {
                List<PhotosForListDTO> photos = await response.Content.ReadAsAsync<List<PhotosForListDTO>>();
                return View(photos);
            }
            else
                return View();
        }
    }
}