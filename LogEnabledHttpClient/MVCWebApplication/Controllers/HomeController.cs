using LogEnabledHttpClient.Extensions;
using Microsoft.AspNetCore.Mvc;
using MVCWebApplication.Models;
using System.Diagnostics;
using System.Net.Http;

namespace MVCWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            using var HttpClient = _httpClientFactory.CreateLogEnabledClient();

            var response = await HttpClient.GetAsync("https://www.google.com");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from Google:\n{content}");
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }


            return View();
        }       
    }
}
